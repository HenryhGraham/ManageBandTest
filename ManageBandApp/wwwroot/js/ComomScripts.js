function DeleteNomenclature(idx) {
    var deleteElementId = 'NomenclaturesToCreate_' + idx + '__Deleted';
    $("#" + deleteElementId)
        .prop('value', 'true');

    var rowElementId = 'NomenclatureRow_' + idx;
    $("#" + rowElementId).hide();
}
