window.ChoosenSelect =
{
    init: function (elementID)
    {
        $(elementID).chosen();
    },

    initWithOnChange: function (elementID, dotnetObject)
    {
        ChoosenSelect.init(elementID);

        ChoosenSelect.OnChange(elementID, dotnetObject);
    },

    OnChange: function (elementID, dotnetObject)
    {
        $(elementID).change(function ()
        {
            var blockchainID = $(this).val();

            console.log("JS: chosen - eventListener: " + blockchainID);

            dotnetObject.invokeMethodAsync('SetBlockchainID', blockchainID);
        });
    }
};