using ManageBandApp.Extentions;
using ManageBandApp.Models;
using ManageBandApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageBandApp.ControllerProcessors
{
    public class GoodsControllerProcessor
    {
        public GoodsControllerProcessor(DbAppContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        private readonly DbAppContext _context;

        public GoodsMovingViewModel GetGoodsMovingViewModel(int intPageNum, int toSkip)
        {
            GoodsMovingViewModel model = new GoodsMovingViewModel();

            int pagesCount = (int)Math.Ceiling((decimal)_context.GoodsMovings.GroupBy(m => m.MovingId).Count() / (decimal)Consts.DefaultPageSize);

            if (pagesCount > intPageNum)
                model.HaveNextPage = true;
            if (intPageNum > 1)
                model.HavePreviousPage = true;

            model.CurrentPage = intPageNum;
            model.Movings = _context
                .GoodsMovings
                .Include(m => m.StockIn)
                .Include(m => m.StockOut)
                .GroupBy(m => m.MovingId)
                .Select(m => m.First())
                .OrderBy(m => m.MovingTime)
                .Skip(toSkip)
                .Take(Consts.DefaultPageSize)
                .ToList();
            return model;
        }

        public void RemoveMoving(Guid movingId)
        {
            GoodsMoving[] deleteEntries = _context
                .GoodsMovings
                .Where(m => m.MovingId == movingId)
                .ToArray();

            if (deleteEntries.Length <= 0)
                throw new Exception($"Не найдено перемещение с идентификатором {movingId}");

            _context.GoodsMovings.RemoveRange(deleteEntries);
            _context.SaveChanges();
        }

        public bool CreateMoving(CreateMovingViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Errors.Clear();
            try
            {
                //убираем удаленные номенклатуры
                if (model.NomenclaturesToCreate != null)
                    model.NomenclaturesToCreate.RemoveAll(DeleteNomenclatureRowPredicate);

                ValidateCreateMovingData(model);
                //проверяем остатки
                CheckRemindersOnStock(model);                
                if (model.Errors.Count > 0)
                    return false;

                IEnumerable<GoodsMoving> movingsToAdd = CreteGoodsMovings(model);
                _context.GoodsMovings.AddRange(movingsToAdd);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                model.Errors.Add($"Произошла непредвиденная ошибка при создании перемещения номенклатуры: {ex.ToString()}");
                return false;
            }            
        }

        private void CheckRemindersOnStock(CreateMovingViewModel model)
        {
            if (model.StockOutId != null && model.StockOutId > 0)
            {
                Stock stockOut = _context.Stocks.Find(model.StockOutId);
                if (stockOut == null)
                    model.Errors.Add("Не найден склад - источник перемещения");
                else
                    foreach (CreateMovingNomenclatureModel nomenclatureModel in model.NomenclaturesToCreate)
                    {
                        Nomenclature nomenclature = _context
                            .Nomenclatures
                            .Find(nomenclatureModel.NomenclatureId);

                        if(nomenclature == null)
                            model.Errors.Add($"Не удалось найти номенклатура по идентификатору '{nomenclatureModel.NomenclatureId}'");
                        else
                        {
                            long reminder = stockOut.GetReminder(nomenclature, _context, model.MovingDate);
                            if(reminder - nomenclatureModel.Count < 0)
                                model.Errors.Add($"На складе {stockOut.Name} не достаточно номенклатуры '{nomenclature.Name}' " +
                                    $"на момент {model.MovingDate.ToString("dd.MM.yyyy")}. Количество: {reminder}");
                        }

                    }
            }
        }

        private bool DeleteNomenclatureRowPredicate(CreateMovingNomenclatureModel nomenclatureModel)
        {
            return nomenclatureModel.Deleted == "true";
        }

        private IEnumerable<GoodsMoving> CreteGoodsMovings(CreateMovingViewModel model)
        {
            Guid movingId = Guid.NewGuid();
            Stock stockIn = _context.Stocks
                .FirstOrDefault(s => model.StockInId != null && s.Id == (int)model.StockInId);
            Stock stockOut = _context.Stocks
                .FirstOrDefault(s => model.StockOutId != null && s.Id == (int)model.StockOutId);
                     
            foreach (CreateMovingNomenclatureModel nomenclatureModel in model.NomenclaturesToCreate)
            {
                Nomenclature nomenclature = _context.Nomenclatures
                    .First(n => n.Id == nomenclatureModel.NomenclatureId);
                yield return CreateGoodsMoving(nomenclature, movingId, stockIn, stockOut, nomenclatureModel.Count, model.MovingDate);
            }
        }

        private GoodsMoving CreateGoodsMoving(Nomenclature nomenclature, Guid movingId, Stock stockIn, Stock stockOut, uint count, DateTime movingDate)
        {
            return new GoodsMoving
            { 
                MovingId = movingId,
                Nomenclature = nomenclature,
                StockIn = stockIn,
                StockOut = stockOut,
                Count = count,
                MovingTime = movingDate
            };
        }

        private void ValidateCreateMovingData(CreateMovingViewModel model)
        {         
            if ((model.StockInId == null || model.StockInId <= 0)
                && (model.StockOutId == null || model.StockOutId <= 0))
                model.Errors.Add("Необходимо выбрать склад хотя бы в одном из пунктов: Откуда идет перемещение или Куда идет перемещение");
            else if (model.StockInId == model.StockOutId)
                model.Errors.Add("Нельзя выбрать один и тот же склад на поступление и расход");

            if (model.NomenclaturesToCreate == null || model.NomenclaturesToCreate.Count <= 0)
                model.Errors.Add("Необходимо добавить номенклатуру");
            {
                if (model.NomenclaturesToCreate.Any(n => n.NomenclatureId == null || n.NomenclatureId <= 0))
                    model.Errors.Add("Необходимо везде выбрать номенклатуру");
                else if(model.NomenclaturesToCreate.GroupBy(n => n.NomenclatureId).Count()
                    < model.NomenclaturesToCreate.Count)
                    model.Errors.Add("Номенклатура не должна повторяться");

                if (model.NomenclaturesToCreate.Any(n => n.Count <= 0))
                    model.Errors.Add("Некорректно заполнено количество. Поле должно быть заполнено целым числом больше нуля");
               
            }
        }
    }
}
