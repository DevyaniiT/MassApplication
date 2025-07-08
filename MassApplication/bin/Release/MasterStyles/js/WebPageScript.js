$(document).ready(function () {
    $("#Duration_List").change(function () {
        var selectedVal = $(this).find(':selected').val();
        //var selectedText = $(this).find(':selected').text();
        if (selectedVal === "daily") {
            $("#Daily").show();
            $("#Hourly").hide();
            $("#Monthly").hide();
            $("#Quarterly").hide();
            $("#Yearly").hide();
            $("#Fortnightly").hide();
            $("#HalfYearly").hide();
            $("#Custom").hide();
        }
        else if (selectedVal === "hourly") {
            $("#Hourly").show();
            $("#Daily").hide();
            $("#Monthly").hide();
            $("#Quarterly").hide();
            $("#Yearly").hide();
            $("#Fortnightly").hide();
            $("#HalfYearly").hide();
            $("#Custom").hide();
        }
        else if (selectedVal === "monthly") {
            $("#Monthly").show();
            $("#Daily").hide();
            $("#Hourly").hide();
            $("#Quarterly").hide();
            $("#Yearly").hide();
            $("#Fortnightly").hide();
            $("#HalfYearly").hide();
            $("#Custom").hide();
        }
        else if (selectedVal === "quarterly") {
            $("#Quarterly").show();
            $("#Daily").hide();
            $("#Hourly").hide();
            $("#Monthly").hide();
            $("#Yearly").hide();
            $("#Fortnightly").hide();
            $("#HalfYearly").hide();
            $("#Custom").hide();
        }
        else if (selectedVal === "yearly") {
            $("#Yearly").show();
            $("#Daily").hide();
            $("#Hourly").hide();
            $("#Monthly").hide();
            $("#Quarterly").hide();
            $("#Fortnightly").hide();
            $("#HalfYearly").hide();
            $("#Custom").hide();
        }
        else if (selectedVal === "fortnightly") {
            $("#Fortnightly").show();
            $("#Yearly").hide();
            $("#Daily").hide();
            $("#Hourly").hide();
            $("#Monthly").hide();
            $("#Quarterly").hide();
            $("#HalfYearly").hide();
            $("#Custom").hide();
        }
        else if (selectedVal === "halfyearly") {
            $("#HalfYearly").show();
            $("#Fortnightly").hide();
            $("#Yearly").hide();
            $("#Daily").hide();
            $("#Hourly").hide();
            $("#Monthly").hide();
            $("#Quarterly").hide();
            $("#Custom").hide();
        }
        else if (selectedVal === "custom") {
            $("#Custom").show();
            $("#Fortnightly").hide();
            $("#Yearly").hide();
            $("#Daily").hide();
            $("#Hourly").hide();
            $("#Monthly").hide();
            $("#Quarterly").hide();
            $("#HalfYearly").hide();
        }
        else {
            $('#Daily').show();
            $("#Hourly").hide();
            $('#Monthly').hide();
            $('#Quarterly').hide();
            $('#Yearly').hide();
            $("#Fortnightly").hide();
            $("#HalfYearly").hide();
            $("#Custom").hide();
        }
    });
});