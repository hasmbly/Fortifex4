window.DataTable =
{
    init: function (elementID)
    {
        $(elementID).DataTable({
            destroy: true
        });
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