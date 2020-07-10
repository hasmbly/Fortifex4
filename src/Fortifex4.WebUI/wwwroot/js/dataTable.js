window.DataTable =
{
    init: function (elementID)
    {
        $(elementID).dataTable();
    },

    destroy: function (elementID)
    {
        $(elementID).dataTable().fnDestroy();
    },

    reset: function (elementID)
    {
        DataTable.destroy(elementID);

        DataTable.init(elementID);
    }
};