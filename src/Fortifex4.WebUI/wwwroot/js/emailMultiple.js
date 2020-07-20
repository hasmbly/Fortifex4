window.EmailMultiple =
{
    init: function (elementID)
    {
        $(elementID).email_multiple({});
    },
    
    getListOfContributor: function (elementID)
    {
        var contributors = $(elementID).val();
        contributors = contributors.split(';');

        console.log("Js: contributors: " + contributors);

        return contributors;
    }
};