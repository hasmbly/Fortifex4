﻿window.Portfolio =
{
    elementID: '#table-portfolio',

    init: function ()
    {
        Portfolio.removeArrow();
        Portfolio.initDataTable();
        Portfolio.setArrow();
    },

    initDataTable: function ()
    {
        $(Portfolio.elementID).DataTable({
            "order": [[7, "desc"]],
            destroy: true
        });
    },

    setArrow: function ()
    {
        $(".number-highlight").each(function ()
        {
            var result = $(this).text();
            var substring = result.substring(0, 1);
            var isEmpty = result.substring(0, 3) === '---';

            if (!isEmpty && substring !== "-")
            {
                $(this).addClass("positive-value");
                $(this).prepend("<img class='arrow-value' src='/images/uparrow.png' />");
            }
            else if (!isEmpty)
            {
                $(this).addClass("minus-value");
                $(this).prepend("<img class='arrow-value' src='/images/downarrow.png' />");
            }
            else
            {
                $(this).removeClass("positive-value");
                $(this).removeClass("minus-value");
                $(this).find(".arrow-value").remove();
            }
        });

        $(".time-frame").each(function ()
        {
            var result = $(this).text();
            var substring = result.substring(0, 1);
            var isEmpty = result.substring(0, 3) === '---';

            if (!isEmpty && substring !== "-")
            {
                $(this).addClass("positive-value");
            }
            else if (!isEmpty)
            {
                $(this).addClass("minus-value");
            }
            else
            {
                $(this).removeClass("positive-value");
                $(this).removeClass("minus-value");
            }
        });

        $(".profit-loss").each(function ()
        {
            var result = $(this).text();
            var substring = result.substring(0, 1);
            var isEmpty = result.substring(0, 3) === '---';

            if (!isEmpty && substring !== "-")
            {
                $(this).addClass("positive-value");
            }
            else if (!isEmpty)
            {
                $(this).addClass("minus-value");
            }
            else
            {
                $(this).removeClass("positive-value");
                $(this).removeClass("minus-value");
            }
        });
    },

    removeArrow: function ()
    {
        $(".number-highlight").each(function ()
        {
            $(this).removeClass("positive-value");
            $(this).removeClass("minus-value");
            $(this).find(".arrow-value").remove();
        });

        $(".time-frame").each(function ()
        {
            $(this).removeClass("positive-value");
            $(this).removeClass("minus-value");
        });

        $(".profit-loss").each(function ()
        {
            $(this).removeClass("positive-value");
            $(this).removeClass("minus-value");
        });
    },
};