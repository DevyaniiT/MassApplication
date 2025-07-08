$(".panel-left").resizable({
    handleSelector: ".splitter",
    resizeHeight: false
});
$(".panel-top").resizable({
    handleSelector: ".splitter-horizontal",
    resizeWidth: false
});

function MyGrid(id) {
    var width = 0;
    var height = 0;
    var cols = 12;
    //var rows = 12;
    var rows = 24;
    var vmargin = 20;

    var lastHeight = 0;

    var gridObj = 0;

    var options = {
        width: cols,
        height: rows,
        float: false,
        animate: true,
        disableResize: false,
        disableDrag: false,
        cellHeight: 'auto',
        cellHeightUnit: 'px',
        removable: '',
        removeTimeout: 100,
        verticalMargin: vmargin,
        acceptWidgets: '.grid-stack-item',
        resizable: { handles: 'e, se, s, sw, w' },
        alwaysShowResizeHandle: /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)
    };

    function init() {
        $(id).gridstack(options);

        gridObj = $(id).data('gridstack');

        gridObj._updateHeightsOnResize = function () { resize(); };

        gridObj.onResizeHandler();
    }

    function resize() {
        width = $(id).parent().width();
        height = $(id).parent().height() - ((rows - 1) * vmargin);

        if (0 >= height) {
            setTimeout(gridObj.onResizeHandler, 1000);
            return;
        }

        if (lastHeight == height)
            return;
            
        lastHeight = height;
        //console.log("height",height);
        // gridObj.cellHeight(parseInt(height / rows) + 'px');

    }

    function addTile(tmpl_tile, mode, x, y, w, h) {
        x = x ? x : 0;
        y = y ? y : 0;
        w = w ? w : 4;
        h = h ? h : 2;

        if (mode == 'edit') {
            gridObj.addWidget(tmpl_tile, x, y, w, h); //editing
            return tmpl_tile;
        }
        

        //var tile = $($(tmpl_tile).text());
        //gridObj.addWidget(tmpl_tile, x, y, w, h); //editing
        gridObj.addWidget(tmpl_tile, x, y, w, h, true); //adding

        return tmpl_tile;
    }
   
    function removeTile(h) {
        gridObj.removeWidget($(h).closest('.grid-stack-item'));
    }

    function save() {
        return $.makeArray($(id + ' > .grid-stack-item:visible')).map(function (v) {
            var n = $(v).data('_gridstack_node');
            return n ? { x: n.x, y: n.y, width: n.width, height: n.height } : null;
        });
    }

    function load(data, tmpl_tile) {
        gridObj.removeAll();
        $.each(data, function (k, v) { addTile(tmpl_tile, v.x, v.y, v.width, v.height); });
    }

    function clear() {
        gridObj.removeAll();
    }

    init();

    this.resize = resize;
    this.addTile = addTile;
    this.removeTile = removeTile;
    this.save = save;
    this.load = load;
    this.clear = clear;
}

var myGrid = 0;
$(function () {
    var tmpl = [
        // {"x":0,"y":4,"width":4,"height":4},
        //{"x":0,"y":8,"width":4,"height":4},
        //{"x":0,"y":0,"width":4,"height":4},
        //{"x":4,"y":0,"width":8,"height":4},
        // {"x":4,"y":4,"width":8,"height":8}
    ];

    myGrid = new MyGrid('#grid', 'tile_');

    //myGrid.load(tmpl, '#tmpl_tile');
});

//$('#addTile').on('click', function () { myGrid.addTile('#tmpl_tile'); });

var D_id = 1;
var chart_id = 1;
var currentElement = "";

function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    //ev.dataTransfer.setData("text", ev.target.id);
    ev.dataTransfer.setData("dataID", ev.target.getAttribute("data-id"));
    ev.dataTransfer.setData("queryID", ev.target.getAttribute("data-val"));
    ev.dataTransfer.setData("chartTYPE", ev.target.getAttribute("data-val-number"));
    ev.dataTransfer.setData("chartNAME", ev.target.getAttribute("data-data-val"));
    //ev.dataTransfer.setData("chartPROPERTY", ev.target.getAttribute("data-content"));
}

function drop(ev) {
    ev.preventDefault();

    var KpiId = ev.dataTransfer.getData("dataID");
    var QueryId = ev.dataTransfer.getData("queryID");
    var chartType = ev.dataTransfer.getData("chartTYPE");
    var chartName = ev.dataTransfer.getData("chartNAME");
    //var chartproperty = ev.dataTransfer.getData("chartPROPERTY");
    var chartproperty = $("#" + KpiId).val();

    var f_name = FontName;
    var f_size = FontSize + 'px';

    stickyID = D_id;
    chartTypeName = chartType;

    reinitializeVariable();
    getDivJsonData(chartTypeName);

    var newNote = "";
    if (chartType.toLowerCase() == "tabular") {

        newNote = $("<div><div class='grid-stack-item-content tile card plainCard' id='sticky" + D_id + "' data-id='" + KpiId + "' data-val-number='" + chartType + "' data-val='" + QueryId + "' data-content='" + chartproperty + "'>"

            //+ "<div class= 'card-header border-0'>"
            //+ "<h3 class='card-title col-11' id='mainHeader" + D_id + "'>" + chartName + ""
            ////+ "<i class='fa fa-th mr-1'></i>" + chartName + ""
            //+ "</h3><div class='card-tools'>"
            //+ "<button id='" + D_id + "' class='btn bg-info btn-sm openbtn' type='button' onclick='openNav(" + D_id + ")' style='margin-right:5px;'>"
            //+ "<i class='fas fa-cog' name=" + chartName + "></i></button>"
            //+ "<button type='button' class='btn bg-info btn-sm removebtn' id='" + D_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
            //+ "<i class='fa fa-times'></i></button>"
            //+ "</div></div>"

            + "<div class='card-header border-0'>"
            + "<div class='d-flex'>"
            + "<h3 class='mr-2 card-title w-100' id='mainHeader" + D_id + "'>" + chartName + "</h3>"
            + "<button id='" + D_id + "' class='btn bg-info btn-sm openbtn' type='button' onclick='openNav(" + D_id + ")' style='margin-right:5px;'>"
            + "<i class='fas fa-cog'></i></button>"
            + "<button type='button' class='btn bg-info btn-sm removebtn' id='" + D_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
            + "<i class='fa fa-times'></i>"
            + "</button></div></div>"


            + "<div class='card-body chartCard" + D_id + "' style='overflow:auto'>"
            + "<table id='myChart" + D_id + "' class='chartCanvas'></table>"
            + "</div></div></div>");

        myGrid.addTile(newNote);
        currentElement = "sticky" + D_id;
        $("#chartRow").append(newNote);
        $('#mainHeader' + D_id).css({
            "font-size": f_size,
            "font-family": f_name
        });
        D_id++;
        GenerateRuntimeGraph(KpiId, chartType, chart_id, QueryId);
        chart_id++;
    }
    else if (chartType.toLowerCase() == "label") {
        newNote = $("<div><div class='grid-stack-item-content tile card plainCard' id='sticky" + D_id + "' data-id='" + KpiId + "' data-val-number='" + chartType + "' data-val='" + QueryId + "' data-content='" + chartproperty + "'>"
            + "<div class= 'card-header border-0'><h3 class='card-title'>"
            + "<i class='fa fa-th mr-1'></i>" + chartName + "</h3>"
            + "<div class='card-tools'>"
            + "<button type='button' class='btn bg-info btn-sm removebtn' id='" + D_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
            + "<i class='fa fa-times'></i>"
            + "</button></div></div>"
            + "<div class='card-body small-boxx chartCard" + D_id + "' id='myChart" + D_id + "'>"
            // + "<div id='myChart" + D_id + "' class='chartCanvas'></div>"
            + "</div></div></div>");

        myGrid.addTile(newNote);
        currentElement = "sticky" + D_id;
        $('#mainHeader' + D_id).css({
            "font-size": f_size,
            "font-family": f_name
        });
        D_id++;
        GenerateRuntimeGraph(KpiId, chartType, chart_id, QueryId);
        chart_id++;

    }
    else {
        newNote = $("<div><div class='grid-stack-item-content tile card plainCard' id='sticky" + D_id + "' data-id='" + KpiId + "' data-val-number='" + chartType + "' data-val='" + QueryId + "' data-content='" + chartproperty + "'>"
            //+ "<div class= 'card-header border-0'><h2 class='card-title'>"
            //+ "<i class='fa fa-th mr-1'></i>" + chartName + "</h2>"
            //+ "<div class='card-tools'>"

            //+ "<div class='card-header border-0'>"
            //+ "<h3 class='card-title col-10' id='mainHeader" + D_id + "'>" + chartName + "</h3>"
            //+ "<div class='card-tools'>"
            //+ "<button id='" + D_id + "' class='btn bg-info btn-sm openbtn' type='button' onclick='openNav(" + D_id + ")' style='margin-right:5px;'>"
            //+ "<i class='fas fa-cog' name=" + chartName + "></i></button>"
            //+ "<button type='button' class='btn bg-info btn-sm daterange' data-toggle='tooltip' title='Date range' style='margin-right:5px;'>"
            //+ "<i class='fa fa-calendar'></i></button>"
            //+ "<button type='button' class='btn bg-info btn-sm removebtn' id='" + D_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
            //+ "<i class='fa fa-times'></i>"
            //+ "</button></div></div>"

            + "<div class='card-header border-0'>"
            + "<div class='d-flex'>"
            + "<h3 class='mr-2 card-title w-100' id='mainHeader" + D_id + "'>" + chartName + "</h3>"
            + "<button id='" + D_id + "' class='btn bg-info btn-sm openbtn' type='button' onclick='openNav(" + D_id + ")' style='margin-right:5px;'>"
            + "<i class='fas fa-cog'></i></button>"
            + "<button type='button' class='btn bg-info btn-sm daterange' data-toggle='tooltip' title='Date range' style='margin-right:5px;'>"
            + "<i class='fa fa-calendar'></i></button>"
            + "<button type='button' class='btn bg-info btn-sm removebtn' id='" + D_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
            + "<i class='fa fa-times'></i>"
            + "</button></div></div>"

            + "<div class='card-body chartCard" + D_id + "'>"
            + "<canvas id='myChart" + D_id + "' class='chartCanvas'></canvas>"
            + "</div></div></div>");

        myGrid.addTile(newNote);
        currentElement = "sticky" + D_id;
        $('#' + currentElement).css('cursor', 'grab');
        $("#chartRow").append(newNote);
        $('#mainHeader' + D_id).css({
            "font-size": f_size,
            "font-family": f_name
        });
        D_id++;
        GenerateRuntimeGraph(KpiId, chartType, chart_id, QueryId);
        chart_id++;
    }

    $('.daterange').daterangepicker({
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        startDate: moment().subtract(29, 'days'),
        endDate: moment()
    }, function (start, end) {
        window.alert('You chose: ' + start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
    });
}

function delTile(tile) {
    var tileId = tile.id;
    myGrid.removeTile(tile);
    closeNav();
    //reinitializeVariable();
    del_property_values(tileId);
}

$('#load').on('click', function () {
    $('#jsonStr').val(JSON.stringify(myGrid.save()));
});

function createdynamiclabel(yourdata, chartname, caption, color, id) {
    var dataValue = '';
    var eleName = "#" + chartname;
    var label_id = "#mylabel" + id;
    var newLabel = "";
    var newDiv = $('<div class="col" id="mylabel' + id + '"></div>').appendTo(eleName);
    for (var i = 0; i < yourdata.length; i++) {
        //var array = yourdata[i].split(/:|}|]/g);
        //dataValue = array[1];
        //dataValue = parseFloat(dataValue).toFixed(5);
        //dataValue = parseFloat(yourdata[i]).toFixed(5);
        dataValue = yourdata[i];
        newLabel = $('<div class="row">'
            + '<div class="col-sm-10">'
            + '<h3 class="headerText" style="font-size:40px; overflow:hidden; font-weight:500; color:#fff; padding:10px;">' + dataValue + '</h3>'
            + '<p class="text-white pl-3 headerText" style="font-size:20px; font-weight:500;">' + caption[i] + '</p>'
            + '</div>'
            //+ '<div class="col-sm-2">'
            //+ '<div class="float-right">'
            //+ '<div class="icon"><i class="fas fa-signal" style="color:#fff; font-size:60px"></i></div>'
            //+ '</div></div>'
            + '</div>');

        $(newDiv).append(newLabel);
    }

    var colorName = '';
    if (color.indexOf("#") < 0)
        colorName = '#' + color;
    else
        colorName = color;

    $(eleName).css("backgroundColor", colorName);
    $(eleName).css("border-radius", "0.25rem");
    $('.iconClass').css("color", "White");
    $('.headerText').css("color", "White");
}
function createdynamicMultilabel(yourdata, chartname, caption, labelposition, color, id) {
    var dataValue = '';
    var eleName = "#" + chartname;
    var newLabel = "";

    var newDiv = $('<div class="container" id="stickyContainer' + id + '"></div>').appendTo(eleName);
    $('#stickyContainer' + id).css("display", "grid");
    //$('#stickyContainer' + id).css("grid-template-columns", "repeat(" + yourdata.length + ", 1fr)");
    //$('#stickyContainer' + id).css("grid-template-rows", "repeat(" + yourdata.length + ", 1fr)");
    $('#stickyContainer' + id).css("grid-template-columns", "repeat(3, 1fr)");
    $('#stickyContainer' + id).css("grid-template-rows", "repeat(3, 1fr)");

    for (var i = 0; i < yourdata.length; i++) {
        //var array = yourdata[i].split(/:|}|]/g);
        //dataValue = array[1];
        //dataValue = parseFloat(dataValue).toFixed(5);

        //dataValue = parseFloat(yourdata[i]).toFixed(5);
        dataValue = yourdata[i];

        newLabel = $('<div class="col" id="mylabel' + id + i + '">'
            + '<div class="row">'
            + '<div class="col-sm-10">'
            + '<h3 class="headerText" style="font-size:40px; overflow:hidden; font-weight:500; color:#fff; padding:10px;">' + dataValue + '</h3>'
            + '<p class="text-white pl-3 headerText" style="font-size:20px; font-weight:500;">' + caption[i] + '</p>'
            + '</div>'
            //+ '<div class="col-sm-2">'
            //+ '<div class="float-right">'
            //+ '<div class="icon"><i class="fas fa-signal" style="color:#fff; font-size:60px"></i></div>'
            //+ '</div></div>'
            + '</div></div>');

        $(newDiv).append(newLabel);

        var alignVal = labelposition[i];
        var alignDataSet = getAlignmentValue(alignVal);
        $('#mylabel' + id + i).css('grid-row', alignDataSet[0]);
        $('#mylabel' + id + i).css('grid-column', alignDataSet[1]);

    }

    var colorName = '';
    if (color.indexOf("#") < 0)
        colorName = '#' + color;
    else
        colorName = color;

    $(eleName).css("backgroundColor", colorName);
}
function getAlignmentValue(alignValue) {
    var alignSet = [];
    if (alignValue == 'ul') {
        alignSet.push('1', '1');
    }
    else if (alignValue == 'um') {
        alignSet.push('1', '2');
    }
    else if (alignValue == 'ur') {
        alignSet.push('1', '3');
    }
    else if (alignValue == 'll') {
        alignSet.push('3', '1');
    }
    else if (alignValue == 'lm') {
        alignSet.push('3', '2');
    }
    else if (alignValue == 'lr') {
        alignSet.push('3', '3');
    }
    else if (alignValue == 'ml') {
        alignSet.push('2', '1');
    }
    else if (alignValue == 'mm') {
        alignSet.push('2', '2');
    }
    else if (alignValue == 'mr') {
        alignSet.push('2', '3');
    }
    else { }

    return alignSet;
}

var myChart;
var myChartConfig;
var chartData = [];
function GenerateRuntimeGraph(query_id, chartname, chartID, QId) {
    var id = query_id;
    var name = chartname;
    $.ajax({
        type: "POST",
        url: 'NewChart?QueryId=' + id,
        success: function (chData) {
            //DrawTheGraph(chData, name, chartID, QId);
            //DrawTheGraph(chData, name, chartID, QId, kpi_pro["TableHeaderColor"], kpi_pro["TableHeaderFontColor"], kpi_pro["TableHeaderFontName"], kpi_pro["TableHeaderFontSize"], kpi_pro["TableHeaderBold"], kpi_pro["TableHeaderItalic"], kpi_pro["TableHeaderUnderline"], kpi_pro["TableHeaderAlign"], kpi_pro["showHeader"], kpi_pro["showHeaderBorderShadow"], kpi_pro["TableCellFontColor"], kpi_pro["TableCellFontName"], kpi_pro["TableCellFontSize"], kpi_pro["TableCellBold"], kpi_pro["TableCellItalic"], kpi_pro["TableCellUnderline"], kpi_pro["TableCellAlign"], kpi_pro["TableCellBorderColor"], kpi_pro["OddCellColor"], kpi_pro["EvenCellColor"], kpi_pro["ShowPagination"], kpi_pro["showLayoutBorderShadow"], kpi_pro["showWrapText"], kpi_pro["showCompactNumber"]);
            DrawTheGraph(chData, name, chartID, QId, TableHeaderColor, TableHeaderFontColor, TableHeaderFontName, TableHeaderFontSize, TableHeaderBold, TableHeaderItalic, TableHeaderUnderline, TableHeaderAlign, showHeader, showHeaderBorderShadow, TableCellFontColor, TableCellFontName, TableCellFontSize, TableCellBold, TableCellItalic, TableCellUnderline, TableCellAlign, TableCellBorderColor, OddCellColor, EvenCellColor, ShowPagination, showLayoutBorderShadow, showWrapText, showCompactNumber);
        },
        "error": function (data) {
            alert("Some Error Occured!");
        }
    });

}

function DrawTheGraph(data, name, chartId, id, tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber) {
    var t = [JSON.parse(data)];
    data = data.replace("#", "");
    if (name == "tabular") {
        $.ajax({
            type: "POST",
            //contentType: false,
            url: 'KpiTabularData?QueryId=' + id + '&chartparams=' + data,
            success: function (modelval) {
                var mainTab = "myChart" + chartId;
                //createdynamictable(modelval, mainTab, chartId);
                createdynamictable(modelval, mainTab, chartId, tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber);
            }
        });
    }
    else if (name == "label") {
        $.ajax({
            type: "POST",
            //url: hostAddress + windowPath + 'DashBoard/KpiLabelData?QueryId=' + id + '&chartparams=' + data),
            url: 'KpiLabelData?QueryId=' + id + '&chartparams=' + data,
            success: function (modelval) {
                var modelData = modelval.Success;
                var modelColor = modelval.Message;
                var modelCaption = modelval.DataVal;
                var modelPosition = modelval.DataVal1;
                var mainTab = "myChart" + chartId;
                if (modelPosition == null || modelPosition == "") {
                    createdynamiclabel(modelData, mainTab, modelCaption, modelColor, chartId);
                }
                else
                {
                    createdynamicMultilabel(modelData, mainTab, modelCaption, modelPosition, modelColor, chartId);
                }
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: 'KpiData?QueryId=' + id,
            success: function (modelval) {
                DrawTheGraphData(t, modelval, name, chartId);
            }
        });
    }
}

function DrawTheGraphData(t, modelval, name, chartId) {
    var xvl;
    var hvl;
    var thvl;
    var gvl;
    var cvlOld;
    var cvl = [];
    var dvl;
    var highvl;
    var lowvl;
    var openvl;
    var closevl;

    var actualHeightData = [];
    var thresholdVal = [];
    var actualColorData = [];
    var myObject = eval('(' + modelval + ')');
    //console.log("myObject:", myObject);
    let obj = Object.keys(t[0]);
    //console.log("obj:", obj);
    let Arr = [];
    for (let i = 0; i < obj.length; i++) {
        let item = obj[i];
        console.log("item:", item);
        if (item == 'Xaxis') {
            xvl = t[0][item];
            let data = {
                key: item,
                value: myObject.map((item) => {
                    return item[xvl];
                })
            }
            Arr.push(data);
        }
        else if (item == 'Height') {
            hvl = t[0][item].split(",");
            for (let j = 0; j < hvl.length; j++) {
                var dataval = hvl[j];
                let data = {
                    key: item,
                    value: myObject.map((item) => {
                        //var number = new Intl.NumberFormat('en-IN').format(item[dataval]);
                        //number = parseFloat((Math.round(item[dataval] * 100) / 100).toFixed(2));
                        //number = parseFloat(Intl.NumberFormat('en-US').format(number));
                        //return number;
                        return item[dataval];
                    })
                }
                actualHeightData.push(data);
                Arr.push(data);
            }
        }
        else if (item == 'Threshold') {
            thvl = t[0][item].split(",");
            for (let j = 0; j < thvl.length; j++) {
                var dataval = thvl[j];
                let data = {
                    key: dataval,
                    value: myObject.map((item) => {
                        return item[dataval];
                    })
                }
                thresholdVal.push(data);
                Arr.push(data);
            }
        }
        else if (item == 'Group') {
            //alert(t[0][item]);
            gvl = t[0][item];
            let data = {
                key: item,
                value: myObject.map((item) => {
                    return item[gvl];
                })
            }
            Arr.push(data);
        }
        else if (item.toLowerCase() == 'color') {
            //cvlOld = t[0][item].split("),");
            cvlOld = t[0][item].split(/,(?![^()]*\))\s*/g);
            for (let j = 0; j < cvlOld.length; j++) {
                var color_dataval = cvlOld[j];
                //color_dataval = color_dataval + ")";
                //color_dataval = color_dataval.replace("))", ")");
                cvl.push(color_dataval);
            }
            //console.log("cvl:", cvl);
        }
        else if (item == 'Date') {
            dvl = t[0][item];
            let data = {
                key: item,
                value: myObject.map((item) => {
                    return item[dvl];
                })
            }
            Arr.push(data);
        }
        else if (item == 'High') {
            openvl = t[0][item];
            let data = {
                key: item,
                value: myObject.map((item) => {
                    return item[openvl];
                })
            }
            Arr.push(data);
        }
        else if (item == 'Low') {
            lowvl = t[0][item];
            let data = {
                key: item,
                value: myObject.map((item) => {
                    return item[lowvl];
                })
            }
            Arr.push(data);
        }
        else if (item == 'Open') {
            openvl = t[0][item];
            let data = {
                key: item,
                value: myObject.map((item) => {
                    return item[openvl];
                })
            }
            Arr.push(data);
        }
        else if (item == 'Close') {
            closevl = t[0][item];
            let data = {
                key: item,
                value: myObject.map((item) => {
                    return item[closevl];
                })
            }
            Arr.push(data);
        }
        else { }
    }
    DrawDynamicGraph(Arr, actualHeightData, thresholdVal, name, xvl, gvl, hvl, cvl, dvl, highvl, lowvl, openvl, closevl, chartId);
}

function DrawDynamicGraph(data, actualHeightData, thresholdVal, name, xval, gval, hval, cval, dval, highval, lowval, openval, closeval, chartId) {
    var Xaxisvalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Xaxis';
    });

    if (result.length > 0) {
        Xaxisvalue = (result[0].value);
    }

    var Hightvalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Height';
    });

    if (result.length > 0) {
        for (let i = 0; i < result.length; i++) {
            //var number = new Intl.NumberFormat('en-IN').format(parseFloat(result[i].value));
            //Hightvalue.push(number);
            Hightvalue.push(result[i].value);
        }
    }

    var threshold = [];
    var result = data.filter(function (element) {
        return element.key == 'Threshold';
    });

    if (result.length > 0) {
        for (let i = 0; i < result.length; i++) {
            threshold.push(result[i].value);
        }
    }

    var Groupvalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Group';
    });

    if (result.length > 0) {
        Groupvalue = (result[0].value);
    }

    var Colorvalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Color';
    });

    if (result.length > 0) {
        Colorvalue = (result[0].value);
    }

    var Datevalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Date';
    });

    if (result.length > 0) {
        Datevalue = (result[0].value);
    }

    var Highvalue = [];
    var result = data.filter(function (element) {
        return element.key == 'High';
    });

    if (result.length > 0) {
        Highvalue = (result[0].value);
    }

    var Lowvalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Low';
    });

    if (result.length > 0) {
        Lowvalue = (result[0].value);
    }

    var Openvalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Open';
    });

    if (result.length > 0) {
        Openvalue = (result[0].value);
    }

    var Closevalue = [];
    var result = data.filter(function (element) {
        return element.key == 'Close';
    });

    if (result.length > 0) {
        Closevalue = (result[0].value);
    }

    change(name, Xaxisvalue, actualHeightData, thresholdVal, Hightvalue, Groupvalue, Colorvalue, Datevalue, Highvalue, Lowvalue, Openvalue, Closevalue, xval, hval, gval, cval, dval, highval, lowval, openval, closeval, chartId);

}

function change(newType, Xaxisvalue, actualHeightData, threshold, Hightvalue, Groupvalue, Colorvalue, Datevalue, Highvalue, Lowvalue, Openvalue, Closevalue, xvll, hvll, gvll, cvll, dval, highvll, lowvll, openvll, closevll, chartId) {
    //console.log("cvll:", cvll);
    if (cvll == null || cvll.length == 0)
    {
        cvll = ["lightblue", "lightgreen", "pink", "#FFD580", "#cb504d", "yellow", "purple", "gray", "violet", "black"];
        //console.log("cvll:", cvll);
    }
    var datasset = [];
    var heightValueData = Hightvalue[0];
    var colors = ["lightblue", "lightgreen", "pink", "#FFD580", "#cb504d", "yellow", "purple", "gray", "violet", "black"]; //#FFD580(orange), #cb504d(light red)
    for (let i = 0; i < actualHeightData.length; i++) {
        datasset.push({
            label: hvll[i],
            backgroundColor: cvll[i],
            borderColor: cvll[i],
            data: actualHeightData[i].value,
        });
    }
    if (newType.toLowerCase() == 'pie' || newType.toLowerCase() == 'doughnut' || newType.toLowerCase() == 'polararea') {
        console.log("cvll:", cvll);
        var config = {
            type: newType,
            data: {
                labels: Xaxisvalue,
                datasets: [
                    {
                        backgroundColor: ["Green", "Blue", "orange", "Purple", "Yellow", "Red", "Black", "purple", "violet", "Gray"],
                        //backgroundColor: ["Green", "Blue", "Gray", "pink", "lightblue", "lightgreen", "#FFD580", "#cb504d", "yellow", "purple", "gray", "violet", "black"],
                        data: heightValueData,
                    },
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        //display: Boolean(legendShow),
                        display: legendShow,
                        position: legendPosition,
                        align: legendAlign,
                        labels: {
                            boxWidth: parseInt(legendFontSize),
                            color: legendColor,
                            font: {
                                size: parseInt(legendFontSize),
                                family: legendFontName,
                                weight: legendStyle
                            }
                        }
                    }
                }
            }
        };
    }
    else if (newType.toLowerCase() == "bubble" || newType.toLowerCase() == "scatter") {
        var xlabel = [];
        var label_data = [];
        while (datasset.length > 0) {
            datasset.pop();
        }
        if (Hightvalue.length > 0) {
            if (newType == "bubble") {
                for (let i = 0; i < actualHeightData.length; i++) {
                    var datal = [];
                    for (let j = 0; j < Xaxisvalue.length; j++) {
                        if (i == 0) {
                            xlabel.push(Xaxisvalue[j]);
                        }
                        datal.push({
                            x: Xaxisvalue[j],
                            y: Hightvalue[i][j],
                            r: 6
                        });
                    }
                    label_data.push({
                        label: hvll[i],
                        data: datal,
                        backgroundColor: cvll[i],// random_rgba()
                    });
                }
            }
            else if (newType == "scatter") {
                for (let i = 0; i < actualHeightData.length; i++) {
                    var datal = [];
                    for (let j = 0; j < Xaxisvalue.length; j++) {
                        if (i == 0) {
                            xlabel.push(Xaxisvalue[j]);
                        }
                        datal.push({
                            x: Xaxisvalue[j],
                            y: Hightvalue[i][j]
                        });
                    }
                    label_data.push({
                        label: hvll[i],
                        data: datal,
                        backgroundColor: cvll[i]
                    });
                }
            }

            var config = {
                type: newType,
                data: {
                    labels: Xaxisvalue,
                    datasets: label_data
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: legendShow,
                            position: legendPosition,
                            align: legendAlign,
                            labels: {
                                boxWidth: parseInt(legendFontSize),
                                color: legendColor,
                                font: {
                                    size: parseInt(legendFontSize),
                                    family: legendFontName,
                                    weight: legendStyle
                                }
                            }
                        },
                        tooltips: {
                            enabled: true
                        }
                    },
                    scales: {
                        x: {
                            stacked: true,
                            display: true,
                            ticks: {
                                //color: 'black',
                                font: {
                                    family: 'arial',
                                    size: 10,
                                    style: 'normal'
                                }
                            },
                            grid: {
                                display: true
                            }
                        },
                        y: {
                            stacked: true,
                            display: true,
                            ticks: {
                                //callback: function (val, index) {
                                //    return val / 100000;
                                //},
                                //color: 'black',
                                font: {
                                    family: 'arial',
                                    size: 10,
                                    style: 'normal'
                                }
                            },
                            grid: {
                                display: true
                            }
                        }
                    }
                }
            };
        }
    }
    else if (newType.toLowerCase() == "line" || newType.toLowerCase() == "bar" || newType.toLowerCase() == "area" || newType.toLowerCase() == "spline") {
        while (datasset.length > 0) {
            datasset.pop();
        }
        var fl = false;
        var sl = false;
        var stked = false;
        var splin = 0;
        if (actualHeightData.length > 0) {
            if (newType.toLowerCase() == "area") {
                fl = true;
                newType = "line";
                sl = false;
            }
            else if (newType.toLowerCase() == "spline") {
                colors = ["red", "yellow", "green", "lightblue", "lightgreen", "pink", "#FFD580", "#cb504d", "purple", "gray", "violet", "black"];

                newType = "line";
                splin = 0.6;
                fl = false;
                sl = false;
                stked = false;
            }

            for (let i = 0; i < actualHeightData.length; i++) {
                datasset.push({
                    label: hvll[i],
                    backgroundColor: cvll[i],
                    borderColor: cvll[i],
                    data: actualHeightData[i].value,
                    lineTension: splin,
                    fill: fl,
                    steppedLine: sl,
                });
            }

            var config = {
                type: newType,
                data: {
                    labels: Xaxisvalue,
                    datasets: datasset
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            //display: Boolean(legendShow),
                            display: legendShow,
                            position: legendPosition,
                            align: legendAlign,
                            labels: {
                                boxWidth: parseInt(legendFontSize),
                                color: legendColor,
                                font: {
                                    size: parseInt(legendFontSize),
                                    family: legendFontName,
                                    weight: legendStyle
                                }
                            }
                        },
                        tooltips: {
                            enabled: true
                        }
                    },
                    scales: {
                        x: {
                            stacked: false,
                            title: {
                                //display: Boolean(x_showtitle),
                                display: x_showtitle,
                                text: x_titlename,
                                color: x_color,
                                font: {
                                    family: x_fontfamily,
                                    size: parseInt(x_fontsize),
                                    weight: x_fontstyle,
                                    lineHeight: 1.2
                                }
                            },
                            ticks: {
                                //display: Boolean(AxisShow),
                                display: AxisShow,
                                color: x_label_color,
                                font: {
                                    family: x_label_fontfamily,
                                    size: parseInt(x_label_fontsize),
                                    weight: x_label_fontstyle,
                                    align: x_label_align
                                }
                            },
                            grid: {
                                //display: Boolean(gridLinesShow)
                                display: gridLinesShow
                            }
                        },
                        y: {
                            stacked: false,
                            title: {
                                //display: Boolean(y_showtitle),
                                display: y_showtitle,
                                text: y_titlename,
                                color: y_color,
                                font: {
                                    family: y_fontfamily,
                                    size: parseInt(y_fontsize),
                                    weight: y_fontstyle,
                                    lineHeight: 1.2
                                }
                            },
                            ticks: {
                                //display: Boolean(AxisShow),
                                display: AxisShow,
                                //userCallback: function (value) {
                                //    return numeral(value).format('$ 0,0')
                                //},
                                color: y_label_color,
                                font: {
                                    family: y_label_fontfamily,
                                    size: parseInt(y_label_fontsize),
                                    weight: y_label_fontstyle,
                                    align: y_label_align
                                }
                            },
                            grid: {
                                //display: Boolean(gridLinesShow)
                                display: gridLinesShow
                            }
                        }
                    }
                }

            };
        }

    }
    else if (newType.toLowerCase() == "line_threshold" || newType.toLowerCase() == "bar_threshold") {
        //var colors = ["Green", "Blue", "Gray", "Purple", "Yellow", "Red", "Black", "purple", "orange", "violet"];
        var colors2 = ["red", "orange", "black", "purple"];
        var datasset = [];
        var fl = false;
        var sl = false;
        var splin = 0;

        if (actualHeightData.length > 0) {

            if (newType.toLowerCase() == "bar_threshold") {
                newType = "bar";
            }
            else if (newType.toLowerCase() == "line_threshold") {
                newType = "line";
            }

            for (let i = 0; i < threshold.length; i++) {
                datasset.push({
                    type: "line",
                    label: threshold[i].key,
                    backgroundColor: cvll[i],
                    borderColor: colors2[i],
                    borderDash: [10, 10],
                    data: threshold[i].value,
                    lineTension: splin,
                    fill: fl,
                    steppedLine: sl,
                });
            }

            for (let i = 0; i < actualHeightData.length; i++) {
                datasset.push({
                    type: newType,
                    label: actualHeightData[i].key,
                    backgroundColor: cvll[i],
                    borderColor: cvll[i],
                    data: actualHeightData[i].value,
                    lineTension: splin,
                    fill: fl,
                    steppedLine: sl,
                });
            }
            console.log("DatasSet_BAR", datasset);

            var config = {
                data: {
                    type: newType,
                    labels: Xaxisvalue,
                    datasets: datasset
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            //display: Boolean(legendShow),
                            display: legendShow,
                            position: legendPosition,
                            align: legendAlign,
                            labels: {
                                boxWidth: parseInt(legendFontSize),
                                color: legendColor,
                                font: {
                                    size: parseInt(legendFontSize),
                                    family: legendFontName,
                                    weight: legendStyle
                                }
                            }
                        },
                        tooltips: {
                            enabled: true
                        }
                    },
                    scales: {
                        x: {
                            stacked: false,
                            title: {
                                //display: Boolean(x_showtitle),
                                display: x_showtitle,
                                text: x_titlename,
                                color: x_color,
                                font: {
                                    family: x_fontfamily,
                                    size: parseInt(x_fontsize),
                                    weight: x_fontstyle,
                                    lineHeight: 1.2
                                }
                            },
                            ticks: {
                                //display: Boolean(AxisShow),
                                display: AxisShow,
                                color: x_label_color,
                                font: {
                                    family: x_label_fontfamily,
                                    size: parseInt(x_label_fontsize),
                                    weight: x_label_fontstyle,
                                    align: x_label_align
                                }
                            },
                            grid: {
                                //display: Boolean(gridLinesShow)
                                display: gridLinesShow
                            }
                        },
                        y: {
                            stacked: false,
                            title: {
                                //display: Boolean(y_showtitle),
                                display: y_showtitle,
                                text: y_titlename,
                                color: y_color,
                                font: {
                                    family: y_fontfamily,
                                    size: parseInt(y_fontsize),
                                    weight: y_fontstyle,
                                    lineHeight: 1.2
                                }
                            },
                            ticks: {
                                //display: Boolean(AxisShow),
                                display: AxisShow,
                                //userCallback: function (value) {
                                //    return numeral(value).format('$ 0,0')
                                //},
                                color: y_label_color,
                                font: {
                                    family: y_label_fontfamily,
                                    size: parseInt(y_label_fontsize),
                                    weight: y_label_fontstyle,
                                    align: y_label_align
                                }
                            },
                            grid: {
                                //display: Boolean(gridLinesShow)
                                display: gridLinesShow
                            }
                        }
                    }
                }
            };
        }
    }

    else if (newType.toLowerCase() == "radar") {
        var radarData = {
            labels: Xaxisvalue,
            datasets: [{
                label: hvll,
                data: heightValueData,
                fill: true,
                backgroundColor: "lightblue",
                borderColor: "blue",
            }]
        };
        var config = {
            type: newType,
            data: radarData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: legendShow,
                        position: legendPosition,
                        align: legendAlign,
                        labels: {
                            boxWidth: parseInt(legendFontSize),
                            color: legendColor,
                            font: {
                                size: parseInt(legendFontSize),
                                family: legendFontName,
                                weight: legendStyle
                            }
                        }
                    },
                },
                elements: {
                    line: {
                        borderWidth: 3
                    }
                }
            }
        };
    }
    else if (newType.toLowerCase() == "stacked_bar") {
        newType = 'bar';

        var config = {
            type: newType,
            data: {
                labels: Xaxisvalue,
                datasets: datasset
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: legendShow,
                        position: legendPosition,
                        align: legendAlign,
                        labels: {
                            boxWidth: parseInt(legendFontSize),
                            color: legendColor,
                            font: {
                                size: parseInt(legendFontSize),
                                family: legendFontName,
                                weight: legendStyle
                            }
                        }
                    },
                    tooltips: {
                        enabled: true
                    }
                },
                scales: {
                    x: {
                        stacked: true,
                        title: {
                            //display: Boolean(x_showtitle),
                            display: x_showtitle,
                            text: x_titlename,
                            color: x_color,
                            font: {
                                family: x_fontfamily,
                                size: parseInt(x_fontsize),
                                weight: x_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: x_label_color,
                            font: {
                                family: x_label_fontfamily,
                                size: parseInt(x_label_fontsize),
                                weight: x_label_fontstyle,
                                align: x_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    },
                    y: {
                        stacked: true,
                        title: {
                            //display: Boolean(y_showtitle),
                            display: y_showtitle,
                            text: y_titlename,
                            color: y_color,
                            font: {
                                family: y_fontfamily,
                                size: parseInt(y_fontsize),
                                weight: y_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: y_label_color,
                            font: {
                                family: y_label_fontfamily,
                                size: parseInt(y_label_fontsize),
                                weight: y_label_fontstyle,
                                align: y_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    }
                }
            }
        };
    }
    else if (newType.toLowerCase() == "step_line") {
        newType = 'line';
        while (datasset.length > 0) {
            datasset.pop();
        }
        for (let i = 0; i < actualHeightData.length; i++) {
            datasset.push({
                label: hvll[i],
                data: actualHeightData[i].value,
                borderColor: cvll[i],
                fill: false,
                stepped: true
            });
        }
        var config = {
            type: newType,
            data: {
                labels: Xaxisvalue,
                datasets: datasset,
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: legendShow,
                        position: legendPosition,
                        align: legendAlign,
                        labels: {
                            boxWidth: parseInt(legendFontSize),
                            color: legendColor,
                            font: {
                                size: parseInt(legendFontSize),
                                family: legendFontName,
                                weight: legendStyle
                            }
                        }
                    },
                    tooltips: {
                        enabled: true
                    }
                },
                scales: {
                    x: {
                        //stacked: true,
                        stacked: false,
                        title: {
                            //display: Boolean(x_showtitle),
                            display: x_showtitle,
                            text: x_titlename,
                            color: x_color,
                            font: {
                                family: x_fontfamily,
                                size: parseInt(x_fontsize),
                                weight: x_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: x_label_color,
                            font: {
                                family: x_label_fontfamily,
                                size: parseInt(x_label_fontsize),
                                weight: x_label_fontstyle,
                                align: x_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    },
                    y: {
                        //stacked: true,
                        stacked: false,
                        title: {
                            //display: Boolean(y_showtitle),
                            display: y_showtitle,
                            text: y_titlename,
                            color: y_color,
                            font: {
                                family: y_fontfamily,
                                size: parseInt(y_fontsize),
                                weight: y_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: y_label_color,
                            font: {
                                family: y_label_fontfamily,
                                size: parseInt(y_label_fontsize),
                                weight: y_label_fontstyle,
                                align: y_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    }
                }
            }
        };
    }

    else if (newType.toLowerCase() == "treemap") {
        let arrayData = [];
        for (let i = 0; i < actualHeightData.length; i++) {
            arrayData.push(actualHeightData[i].key);
        }
        let arr = [];
        Xaxisvalue.map((val, index) => {
            let obj = { [xvll]: val, [hvll]: Hightvalue[0][index] };
            arr.push(obj);
        });

        var config = {
            type: newType,
            data: {
                datasets: [{
                    //label: hvll,
                    tree: arr,
                    key: hvll,
                    groups: [xvll],
                    spacing: 0.5,
                    borderWidth: 1.5,
                    fontColor: "black",
                    fontSize: 9,
                    borderColor: "grey",
                    backgroundColor: colors[0],
                    hoverBackgroundColor: colors[1],
                    //backgroundColor: 'rgba(75, 00, 150, 0.2)',
                    rtl: false // control in which direction the squares are positioned
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: legendShow
                    },
                    tooltips: {
                        callbacks: {
                            title: function (item, data) {
                                return data.datasets[item[0].datasetIndex].key;
                            },
                            label: function (item, data) {
                                var dataset = data.datasets[item.datasetIndex];
                                var dataItem = dataset.data[item.index];
                                return dataItem.g + ': ' + dataItem.v;
                            }
                        }
                    }
                }
            }
        }
    }
    else if (newType.toLowerCase() == "histogram") {
        newType = 'bar';
        while (datasset.length > 0) {
            datasset.pop();
        }
        for (let i = 0; i < actualHeightData.length; i++) {
            datasset.push({
                label: hvll[i],
                data: actualHeightData[i].value,
                backgroundColor: cvll[i],
                borderColor: cvll[i],
                borderWidth: 1,
                barPercentage: 1.3
            });
        }

        var histoData = {
            labels: Xaxisvalue,
            datasets: datasset
        };
        var config = {
            type: newType,
            data: histoData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: legendShow,
                        position: legendPosition,
                        align: legendAlign,
                        labels: {
                            boxWidth: parseInt(legendFontSize),
                            color: legendColor,
                            font: {
                                size: parseInt(legendFontSize),
                                family: legendFontName,
                                weight: legendStyle
                            }
                        }
                    },
                    tooltips: {
                        enabled: true
                    }
                },
                scales: {
                    x: {
                        stacked: false,
                        title: {
                            //display: Boolean(x_showtitle),
                            display: x_showtitle,
                            text: x_titlename,
                            color: x_color,
                            font: {
                                family: x_fontfamily,
                                size: parseInt(x_fontsize),
                                weight: x_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: x_label_color,
                            font: {
                                family: x_label_fontfamily,
                                size: parseInt(x_label_fontsize),
                                weight: x_label_fontstyle,
                                align: x_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    },
                    y: {
                        stacked: false,
                        title: {
                            //display: Boolean(y_showtitle),
                            display: y_showtitle,
                            text: y_titlename,
                            color: y_color,
                            font: {
                                family: y_fontfamily,
                                size: parseInt(y_fontsize),
                                weight: y_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: y_label_color,
                            font: {
                                family: y_label_fontfamily,
                                size: parseInt(y_label_fontsize),
                                weight: y_label_fontstyle,
                                align: y_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    }
                }
            }
        };

    }
    else if (newType.toLowerCase() == "candlestick") {

        var dateData = ["2020-01-01", "2020-02-01", "2020-03-01", "2020-04-01", "2020-05-01", "2020-06-01", "2020-07-01", "2020-08-01", "2020-09-01", "2020-10-01", "2020-11-01", "2020-12-01"];
        let arr = [];
        dateData.map((val, index) => {
            let obj = {
                x: luxon.DateTime.fromSQL(dateData[index])["ts"],
                o: Openvalue[index],
                h: Highvalue[index],
                l: Lowvalue[index],
                c: Closevalue[index]
            };
            arr.push(obj);
        });

        var config = {
            type: newType,
            data: {
                datasets: [{
                    label: 'CHRT - Chart.js Corporation',
                    data: arr
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    //legend: {
                    //    display: legendShow
                    //},
                    legend: {
                        display: false
                    },
                    tooltips: {
                        enabled: true
                    }
                },
                scales: {
                    x: {
                        stacked: false,
                        title: {
                            //display: Boolean(x_showtitle),
                            display: x_showtitle,
                            text: x_titlename,
                            color: x_color,
                            font: {
                                family: x_fontfamily,
                                size: parseInt(x_fontsize),
                                weight: x_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: x_label_color,
                            font: {
                                family: x_label_fontfamily,
                                size: parseInt(x_label_fontsize),
                                weight: x_label_fontstyle,
                                align: x_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    },
                    y: {
                        stacked: false,
                        title: {
                            //display: Boolean(y_showtitle),
                            display: y_showtitle,
                            text: y_titlename,
                            color: y_color,
                            font: {
                                family: y_fontfamily,
                                size: parseInt(y_fontsize),
                                weight: y_fontstyle,
                                lineHeight: 1.2,
                            }
                        },
                        ticks: {
                            //display: Boolean(AxisShow),
                            display: AxisShow,
                            color: y_label_color,
                            font: {
                                family: y_label_fontfamily,
                                size: parseInt(y_label_fontsize),
                                weight: y_label_fontstyle,
                                align: y_label_align,
                            }
                        },
                        grid: {
                            //display: Boolean(gridLinesShow)
                            display: gridLinesShow
                        }
                    }
                }
            }
        };
    }
    else if (newType.toLowerCase() == "gauge") {
        newType = 'doughnut';

        var randomValue = function (data) {
            return parseFloat(Math.max.apply(null, data) * Math.random()).toFixed(2);
        };

        var sum = heightValueData.reduce(function (a, b) { return a + b; }, 0);

        var needle_data_val = randomValue(heightValueData) / sum * 100;
        needle_data_val = Math.round(needle_data_val);

        if (Xaxisvalue.length != 0) {
            var GaugeData = {
                labels: Xaxisvalue,
                datasets: [
                    {
                        label: hvll,
                        backgroundColor: ["lightblue", "lightgreen", "pink", "#FFD580", "#cb504d", "yellow", "orange", "purple", "violet", "blue", "green", "red"],
                        data: heightValueData,
                        needleValue: randomValue(heightValueData),
                        borderColor: 'Black',
                        borderWidth: 1,
                        //cutout: '90%',
                        circumference: 180,
                        rotation: 270
                    }
                ]
            };
        }


        var gaugeNeedle = {
            id: 'gaugeNeedle',
            afterDatasetDraw(chart, args, options) {
                var { ctx, config, data, chartArea: { top, bottom, left, right, width, height } } = chart;
                ctx.save();
                var needleValue = data.datasets[0].needleValue;
                var dataTotal = data.datasets[0].data.reduce((a, b) => a + b, 0);
                var angle = Math.PI + (1 / dataTotal * needleValue * Math.PI);
                var cx = width / 2;
                var cy = (chart._metasets[0].data[0].y - 10);

                //needle
                ctx.translate(cx, cy);
                ctx.beginPath();
                ctx.rotate(angle);
                ctx.moveTo(0, -10);
                ctx.lineTo(height - (ctx.canvas.offsetTop - 40), 10);
                ctx.lineTo(0, 10);
                ctx.fillStyle = '#444';
                ctx.fill();
                ctx.restore();

                // needle dot
                //ctx.translate(-cx,-cy);
                ctx.beginPath();
                ctx.arc(cx, cy, 5, 0, 10);
                ctx.fill();
                ctx.restore();

                ctx.font = '20px black';
                ctx.fillStyle = '#444';
                ctx.fillText(needleValue + '%', cx + 20, cy - 90);
                ctx.textAlign = 'center';
                ctx.restore();
            }
        }

        var config = {
            type: newType,
            data: GaugeData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        enabled: true,
                        callbacks: {
                            label: function (context) {
                                //return context['dataset']['label'] + ": " + context['dataset']['data'][context.dataIndex];
                                return context.chart.data.labels[context.dataIndex] + ":" + context['dataset']['data'][context.dataIndex];
                            }
                        }
                    }
                }
            },
            plugins: [gaugeNeedle]
        };

    }

    var chartElement = "myChart" + chartId;
    var ctx = document.getElementById(chartElement).getContext('2d');
    chartData.push({
        key: chartElement,
        value: config
    });

    var temp = jQuery.extend(true, {}, config);
    temp.type = newType;
    myChart = new Chart(ctx, temp);
};

function updateLegends(chart, chart_type, fontColor, fontSize, fontFamily, labelAlign, legendPosition, legendStyle, showlegend, xAxisFontSize, yAxisFontSize, showAxis, showgridLines, yshowtitle, ytitlename, xshowtitle, xtitlename, yColor, yFontFamily, yFontSize, yFontStyle, yAlign, xColor, xFontFamily, xFontSize, xFontStyle, xAlign, y_LabelColor, x_LabelColor, y_LabelFontFamily, x_LabelFontFamily, y_LabelFontSize, x_LabelFontSize, y_LabelFontStyle, x_LabelFontStyle, y_LabelAlign, x_LabelAlign) {

    var Xaxisvalue = [];
    var updateGraph;
    var stackedVal = false;

    var result = chartData.filter(function (element) {
        return element.key == chart;
    });
    if (result.length > 0) {
        Xaxisvalue = (result[0].value);
    }
    //var chart_datatype = Xaxisvalue["type"];
    var chart_datatype = chart_type;
    //console.log("chart_datatype:", chart_datatype);
    split_string = chart.split(/(\d+)/);
    //console.log("Text:" + split_string[0] + " & Number:" + split_string[1]);
    $("canvas#" + chart).remove();
    $("div.chartCard" + split_string[1]).append('<canvas id=' + chart + ' class="chartCanvas"></canvas>');

    var ctx = document.getElementById(chart).getContext('2d');
    var temp = jQuery.extend(true, {}, Xaxisvalue);
    //temp.type = chart_datatype;
    temp.type = Xaxisvalue["type"];
    updateGraph = new Chart(ctx, temp);

    if (chart_datatype.toLowerCase() == 'pie' || chart_datatype.toLowerCase() == 'doughnut' || chart_datatype.toLowerCase() == 'polararea') {
        updateGraph.options = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: showlegend,
                    position: legendPosition,
                    align: labelAlign,
                    labels: {
                        boxWidth: fontSize,
                        color: fontColor,
                        font: {
                            size: fontSize,
                            family: fontFamily,
                            //style: legendStyle,
                            weight: legendStyle
                        }
                    }
                }
            }
        };
    }
    else if (chart_datatype.toLowerCase() == 'radar') {
        updateGraph.options = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: showlegend,
                    position: legendPosition,
                    align: labelAlign,
                    labels: {
                        boxWidth: fontSize,
                        color: fontColor,
                        font: {
                            size: fontSize,
                            family: fontFamily,
                            //style: legendStyle,
                            weight: legendStyle
                        }
                    }
                },
                tooltips: {
                    "callbacks": {
                        "title": (tooltipItem, data) => data.labels[tooltipItem[0].index]
                    }
                }
            },
            elements: {
                line: {
                    borderWidth: 3
                }
            }
        };
    }
    else if (chart_datatype.toLowerCase() == "bubble" || chart_datatype.toLowerCase() == "scatter") {
        updateGraph.options = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: showlegend,
                    position: legendPosition,
                    align: labelAlign,
                    labels: {
                        boxWidth: fontSize,
                        color: fontColor,
                        font: {
                            size: fontSize,
                            family: fontFamily,
                            //style: legendStyle,
                            weight: legendStyle
                        }
                    }
                },
                tooltips: {
                    "callbacks": {
                        "title": (tooltipItem, data) => data.labels[tooltipItem[0].index]
                    }
                }
            },
            scales: {
                x: {
                    stacked: stackedVal,
                    //display: showgridLines,
                    title: {
                        display: xshowtitle,
                        text: xtitlename,
                        color: xColor,
                        font: {
                            family: xFontFamily,
                            size: xFontSize,
                            weight: xFontStyle,
                            lineHeight: 1.2,
                        }
                    },
                    ticks: {
                        display: showAxis,
                        //callback: function (val, index) {
                        //    return val / 100000;
                        //},
                        color: x_LabelColor,
                        font: {
                            family: "'" + x_LabelFontFamily + "'",
                            size: x_LabelFontSize,
                            weight: x_LabelFontStyle,
                            align: x_LabelAlign,
                        }
                    },
                    grid: {
                        display: showgridLines
                    }
                },
                y: {
                    stacked: stackedVal,
                    //display: showgridLines,
                    title: {
                        display: yshowtitle,
                        text: ytitlename,
                        color: yColor,
                        font: {
                            family: yFontFamily,
                            size: yFontSize,
                            weight: yFontStyle,
                            lineHeight: 1.2,
                        }
                    },
                    ticks: {
                        display: showAxis,
                        //callback: function (val, index) {
                        //    return val / 100000;
                        //},
                        color: y_LabelColor,
                        font: {
                            family: "'" + y_LabelFontFamily + "'",
                            size: y_LabelFontSize,
                            weight: y_LabelFontStyle,
                            align: y_LabelAlign
                        }
                    },
                    grid: {
                        display: showgridLines
                    }
                }
            }
        };
    }
    else {
        //console.log("Xaxisvalue:", Xaxisvalue);
        //console.log("Xaxisvalue:", Xaxisvalue["options"]);
        stackedVal = Xaxisvalue["options"]["scales"]["x"]["stacked"];
        //console.log("stackedVal", stackedVal);

        //console.log(Xaxisvalue["options"]["scales"]["xAxes"][0]["barPercentage"]);

        updateGraph.options = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: showlegend,
                    position: legendPosition,
                    align: labelAlign,
                    labels: {
                        boxWidth: fontSize,
                        color: fontColor,
                        font: {
                            size: fontSize,
                            family: fontFamily,
                            //style: legendStyle,
                            weight: legendStyle
                        }
                    }
                },
                tooltips: {
                    enabled: true
                }
            },
            scales: {
                x: {
                    stacked: Boolean(stackedVal),
                    //display: showgridLines,
                    title: {
                        display: xshowtitle,
                        text: xtitlename,
                        color: xColor,
                        font: {
                            family: xFontFamily,
                            size: xFontSize,
                            weight: xFontStyle,
                            lineHeight: 1.2,
                        }
                    },
                    ticks: {
                        display: showAxis,
                        color: x_LabelColor,
                        font: {
                            family: "'" + x_LabelFontFamily + "'",
                            size: x_LabelFontSize,
                            weight: x_LabelFontStyle,
                            align: x_LabelAlign,
                        }
                    },
                    grid: {
                        display: showgridLines
                    }
                },
                y: {
                    stacked: Boolean(stackedVal),
                    //display: showgridLines,
                    title: {
                        display: yshowtitle,
                        text: ytitlename,
                        color: yColor,
                        font: {
                            family: yFontFamily,
                            size: yFontSize,
                            weight: yFontStyle,
                            lineHeight: 1.2,
                        }
                    },
                    ticks: {
                        display: showAxis,
                        //callback: function (val, index) {
                        //    return val / 100000;
                        //},
                        color: y_LabelColor,
                        font: {
                            family: "'" + y_LabelFontFamily + "'",
                            size: y_LabelFontSize,
                            weight: y_LabelFontStyle,
                            align: y_LabelAlign
                        }
                    },
                    grid: {
                        display: showgridLines
                    }
                }
            }
        };
    }

    updateGraph.update();

};

var FontName = '';
var FontSize = '';
function getViewbagData(fontname, fontsize) {
    FontName = fontname;
    FontSize = fontsize;
}

var colTypes = [];
var colNames = [];
var StickyTablename = '';
var stickyID = '';
var chartTypeName = '';
var numericData = [];
var json_DataProperty = [];
var current_JSONDataProperty;

//same heading property
var cardColor = '#fff';
var headingText = '';
var headingColor = '#000';
var headingFontName = FontName;
var headingFontSize = FontSize;
var headingBold = 'normal';
var headingItalic = 'normal';
var headingUnderline = 'normal';
var headingAlign = 'left';
var IconShow = Boolean(false);

//for table heading property
//var TableHeaderColor = '#fff';
var TableHeaderColor = '#fff';
var TableHeaderFontColor = '#000';
var TableHeaderFontName = FontName;
var TableHeaderFontSize = '10';
var TableHeaderBold = 'normal';
var TableHeaderItalic = 'normal';
var TableHeaderUnderline = 'normal';
var TableHeaderAlign = 'left';
var showHeader = Boolean(true);
var showHeaderBorderShadow = Boolean(false);
var TableCellFontColor = '';
var TableCellFontName = FontName;
var TableCellFontSize = '10';
var TableCellBold = 'normal';
var TableCellItalic = 'normal';
var TableCellUnderline = 'normal';
var TableCellAlign = 'left';
var TableCellBorderColor = '#ccc';
var OddCellColor = '';
var EvenCellColor = '';
var ShowPagination = Boolean(true);
var showLayoutBorderShadow = Boolean(false);
var showWrapText = Boolean(false);
var showCompactNumber = Boolean(false);

//for pie chart legend property
var legendColor = '#000';
var legendFontSize = '10';
var legendFontName = FontName;
var legendAlign = 'center';
var legendPosition = 'left';
var legendStyle = 'normal';

//for bar chart legend property
var legendShow = Boolean(false);
var xAxisFontSize = '10';
var yAxisFontSize = '10';
var AxisShow = Boolean(true);
var gridLinesShow = Boolean(true);
var y_showtitle = Boolean(false);
var x_showtitle = Boolean(false);
var y_titlename = '';
var x_titlename = '';
var y_color = '#000';
var x_color = '#000';
var y_fontfamily = FontName;
var x_fontfamily = FontName;
var y_fontsize = '10';
var x_fontsize = '10';
var y_fontstyle = 'normal';
var x_fontstyle = 'normal';
var y_align = 'center';
var x_align = 'center';

var y_label_color = '#000';
var x_label_color = '#000';
var y_label_fontfamily = FontName;
var x_label_fontfamily = FontName;
var y_label_fontsize = '10';
var x_label_fontsize = '10';
var y_label_fontstyle = 'normal';
var x_label_fontstyle = 'normal';
var y_label_align = 'center';
var x_label_align = 'center';

//Function for dynamically created table using json data
function createdynamictable(yourdata, chartname, id, tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber) {
    checkTabularValue(tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber);
    OddCellColor = oddcellcolor;
    EvenCellColor = evencellcolor;
    var tablename = 'mytable' + id;
    var parsedata = JSON.parse(yourdata);

    var table = document.getElementById(chartname);
    //var table = document.createElement("table");  // CREATE DYNAMIC TABLE.
    table.setAttribute("id", tablename);
    table.classList.add('table');

    var arrItems = [];      // THE ARRAY TO STORE JSON ITEMS.
    $.each(parsedata, function (index, value) {
        arrItems.push(value);       // PUSH THE VALUES INSIDE THE ARRAY.
    });

    // EXTRACT VALUE FOR TABLE HEADER.
    var col = [];
    var colType = [];
    for (var i = 0; i < arrItems.length; i++) {
        for (var key in arrItems[i]) {
            if (col.indexOf(key) === -1) {
                col.push(key);
                colType.push(typeof arrItems[0][key]);
            }
        }
    }
    colTypes = colType;

    // CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE.
    var thead = table.createTHead();
    var tr = '';
    tr = thead.insertRow(0);
    for (var i = 0; i < col.length; i++) {
        var th = document.createElement("th");
        //th.setAttribute("style", "font-weight:normal");
        th.style.fontWeight = 'normal';
        th.innerHTML = col[i].toUpperCase();
        tr.appendChild(th);
    }

    var tbody = table.createTBody();
    for (var i = 0; i < arrItems.length; i++) {

        tr = tbody.insertRow(0);

        for (var j = 0; j < col.length; j++) {
            var tabCell = tr.insertCell(-1);
            tabCell.innerHTML = arrItems[i][col[j]];
        }
    }

    var headerName = Object.keys(parsedata[0]);
    // FINALLY ADD THE NEWLY CREATED TABLE WITH JSON DATA TO A CONTAINER.
    //var div = document.getElementById(chartname);
    //div.innerHTML = "";
    //div.appendChild(table);

    var columns = [];
    for (let i = 0; i < headerName.length; i++) {
        columns.push({ "data": headerName[i] });
    }
    colNames = columns;

    $('#' + tablename + ' thead').css("background-color", tableheadercolor);
    $('#' + tablename + ' thead tr th').css({
        "color": tableheaderfontcolor,
        "font-size": tableheaderfontsize + 'px',
        "font-family": tableheaderfontname,
        "font-weight": tableheaderbold,
        "font-style": tableheaderitalic,
        "text-decoration": tableheaderunderline,
        "text-align": tableheaderalign
    });

    var tableData = $('#' + tablename).DataTable(getDataTableDef(columns), showpagination, oddcellcolor, evencellcolor);
    tableData.$('td').css('color', tablecellfontcolor);
    tableData.$('td').css("fontFamily", tablecellfontname);
    tableData.$('td').css('font-size', tablecellfontsize + 'px');
    tableData.$('td').css("font-weight", tablecellbold);
    tableData.$('td').css("font-style", tablecellitalic);
    tableData.$('td').css("text-decoration", tablecellunderline);
    tableData.$('td').css('border-color', tablecellbordercolor);
    //tableData.$('td').css("font-align", tablecellalign);
}
function getDataTableDef(columnDef, show_page, odd_color, even_color) {
    var datatableFormat = {
        columns: columnDef,
        "paging": show_page,
        //"pagingType": "full_numbers",
        //"pageLength": 5,
        //"autoWidth": true,
        "searching": false,   // Search Box will Be Disabled
        "ordering": true,    // Ordering (Sorting on Each Column)will Be Disabled
        "info": false,         // Will show "1 to n of n entries" Text at bottom
        "lengthChange": false, // Will Disabled Record number per page
        language: {
            oPaginate: {
                sNext: '<i class="fa fa-forward"></i>',
                sPrevious: '<i class="fa fa-backward"></i>'

            }
        },
        rowCallback: function (row, data, index) {
            if (index % 2 == 0) {
                $(row).css('background-color', OddCellColor);
            }
            else {
                $(row).css('background-color', EvenCellColor);
            }
        }
    };
    return datatableFormat;
}

//For opening the sidebar
function openNav(id) {
    //console.log("ID:",id);
    closeBtn();
    stickyID = id;
    chartTypeName = $("#sticky" + id).attr("data-val-number");
    $('#mySidebar').html('');
    //var chartname = $('#' + id).find('i').attr("name");
    //var chartname = $('#mainHeader' + id).html();
    var chartname = $('#mainHeader' + id).text();
    var kpi_id = $("#sticky" + id).attr("data-id");
    StickyTablename = chartname;
    headingText = StickyTablename;
    $(".propertyBar").css('display', 'block');
    $(".propertyBar").css('width', '250px');
    var tmp = "";
    var coltmp = "";
    var html_table_data = "";
    var subhtml = "";
    var bRowStarted = true;
    var tableName = 'mytable' + id;
    var tableHeader = [];
    $('#' + tableName + ' thead tr').each(function () {
        // $('#' + tableName + ' tbody tr').each(function () {
        $('th', this).each(function () {
            if (html_table_data.length == 0 || bRowStarted == true) {
                html_table_data += $(this).text();
                //subhtml = subhtml + "<option value=" + $(this).text() + " selected> " + $(this).text() + "  </option> ";
                bRowStarted = false;
            }
            else
                html_table_data += " | " + $(this).text();
            //subhtml = subhtml + "<option value=" + $(this).text() + "> " + $(this).text() + "  </option> ";
            tableHeader.push($(this).text());
        });

    });

    for (let i = 0; i < tableHeader.length; i++) {
        tmp = tableHeader[i];
        coltmp = colTypes[i];
        if (i == 0)
            subhtml = subhtml + "<option value=" + i + " data-id=" + coltmp + " selected> " + tmp + "  </option>";
        else
            subhtml = subhtml + "<option value=" + i + " data-id=" + coltmp + "> " + tmp + "  </option>";

    }

    //var chartPropertyJson = $('#sticky' + id).attr('data-content');
    var chartPropertyJson = $('#' + kpi_id).val();
    chartPropertyJson = JSON.parse(chartPropertyJson);
    var json_obj = [];
    json_obj.push(chartPropertyJson);
    var tablehtmldata = generateTableHtml(json_obj, id, subhtml, chartname, tableName);

    //property_values(json_DataProperty, id);
    property_values(id);

    //append html in sidebar div
    $('#mySidebar').append(tablehtmldata);

    $("#LegendPositionList" + id + " > [value='" + legendPosition + "']").attr("selected", "true");
    if (y_showtitle == true) { $('#Yaxis' + id).val(y_titlename); }
    if (x_showtitle == true) { $('#Xaxis' + id).val(x_titlename); }
    //$('#Xaxis' + id).val(x_titlename);
    //$('#Yaxis' + id).val(y_titlename);
    $('#AxisShow').prop('checked', AxisShow);
    $('#GridlinesShow').prop('checked', gridLinesShow);
    $('#showchartlegend').prop('checked', legendShow);
    $('#IconShow').prop('checked', IconShow);

    $('#TableBorderShadow').prop('checked', showLayoutBorderShadow);
    $('#ShowTablePagination').prop('checked', ShowPagination);
    $('#TableHeaderBorderShadow').prop('checked', showHeaderBorderShadow);
    $('#ShowTableHeader').prop('checked', showHeader);
    $('#WrapHeaderText').prop('disabled', 'disabled');
    $('#TableCompactFormat').prop('disabled', 'disabled');
    //$('#WrapHeaderText').prop('checked', 'showWrapText');
    //$('#TableCompactFormat').prop('checked', 'showCompactNumber');

    //Unchecked all radio buttons when item selected in dropdown list item
    $("#columnList" + id).change(function () {
        $("input[name=" + tableName + "]:checked").prop("checked", false);
    });

    //change legend position according to selection of dropdown list item
    $("#LegendPositionList" + id).change(function () {
        var position = $(this).find('option:selected').val();
        legendPosition = position;
        //var eyeEffect = $('#showchartlegend').is(':checked');
        var chart_ele = 'myChart' + id;
        checkCheckedButton();
        //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    });

    //For changing the color of odd cell
    var OddCellColorValue = '';
    var EvenCellColorValue = '';
    var TableHeaderColorValue = '';
    if (OddCellColor == '') {
        OddCellColorValue = '#fff';
    }
    else {
        OddCellColorValue = OddCellColor;
    }
    if (EvenCellColor == '') {
        EvenCellColorValue = '#fff';
    }
    else {
        EvenCellColorValue = EvenCellColor;
    }
    if (TableHeaderColor == '') {
        TableHeaderColorValue = '#fff';
    }
    else {
        TableHeaderColorValue = TableHeaderColor;
    }
    $(".OddCellColorPicker").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /*color: OddCellColor,*/
        color: OddCellColorValue,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            OddCellColor = color.toHexString();
            //$('#' + tableName + ' tbody tr:nth-child(odd)').css("background-color", color.toHexString());
            //$('#' + tableName).DataTable({
            //    destroy: true,
            //    paging: true,
            //    searching: false,
            //    ordering: true,
            //    info: false,
            //    lengthChange: false,
            //    rowCallback: function (row, data, index) {
            //        //var RowClass = row.getAttribute('class');
            //        //if (RowClass.toLowerCase() == 'odd') {
            //        //    $(row).css('background-color', color.toHexString());
            //        //}

            //        if (index % 2 == 0) {
            //            $(row).css('background-color', color.toHexString());
            //            //$(row).css('background-color', OddCellColor);
            //        }
            //        else {
            //            $(row).css('background-color', EvenCellColor);
            //        }
            //    }
            //});


            var table = $('#' + tableName).DataTable();
            table.$('tr.odd').css('background-color', OddCellColor);
            //table.$('tr.odd').css('background-color', color.toHexString());

        }
    });

    //For changing the color of even cell //DATATBLE//
    $(".EvenCellColorPicker").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /* color: EvenCellColor,*/
        color: EvenCellColorValue,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            EvenCellColor = color.toHexString();
            //$('#' + tableName + ' tbody tr:nth-child(even)').css("background-color", color.toHexString());
            //$('#' + tableName + ' tbody tr:nth-child(odd)').not(':first').css("background-color", color.toHexString());
            $('#' + tableName).DataTable({
                destroy: true,
                paging: true,
                searching: false,
                ordering: true,
                info: false,
                lengthChange: false,
                language: {
                    oPaginate: {
                        sNext: '<i class="fa fa-forward"></i>',
                        sPrevious: '<i class="fa fa-backward"></i>'

                    }
                },
                rowCallback: function (row, data, index) {
                    var RowClass = row.getAttribute('class');
                    if (RowClass.toLowerCase() == 'even') {
                        //$(row).css('background-color', color.toHexString());
                        $(row).css('background-color', EvenCellColor);
                    }

                    if (index % 2 != 0) {
                        //$(row).css('background-color', EvenCellColor);
                        //$(row).css('background-color', color.toHexString());
                        $(row).css('background-color', EvenCellColor);
                    }
                    else {
                        $(row).css('background-color', OddCellColor);
                    }
                }
            });

            //var table = $('#' + tableName).DataTable();
            //table.$('tr.even').css('background-color', color.toHexString());
        }
    });

    //For changing the color of table cell border
    $(".CellBorderColorPicker").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /*color: 'blanchedalmond',*/
        color: TableCellBorderColor,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            //$('#' + tableName + ' tbody tr td').css("border-color", color.toHexString());
            //$('#' + tableName).DataTable({
            //    destroy: true,
            //    paging: true,
            //    searching: false,
            //    ordering: true,
            //    info: false,
            //    lengthChange: false,
            //    rowCallback: function (row, data, index) {
            //        $('td', row).css('border-color', color.toHexString());
            //    }
            //});
            TableCellBorderColor = color.toHexString();
            var table = $('#' + tableName).DataTable();
            table.$('td').css('border-color', TableCellBorderColor);
            //table.$('td').css('border-color', color.toHexString());

        }
    });

    //For changing the color of table header background
    $(".BackgroundColorPicker").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /*color: 'blanchedalmond',*/
        color: TableHeaderColorValue,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            TableHeaderColor = color.toHexString();
            $('#' + tableName + ' thead').css("background-color", TableHeaderColor);
        }
    });

    //For changing the color of table header font
    $(".TableHeaderColorPicker").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /* color: '#fff',*/
        color: TableHeaderFontColor,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            TableHeaderFontColor = color.toHexString();
            $('#' + tableName + ' thead tr th').css("color", TableHeaderFontColor);
        }
    });

    //For changing the background color of table card
    $(".CardBackgroundColorPicker").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /*color: 'blanchedalmond',*/
        color: cardColor,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            //$('#mainHeader' + id).css("color", color.toHexString());
            cardColor = color.toHexString();
            $('#sticky' + id).css("background-color", cardColor);
            //$('#sticky' + id).css("background-color", color.toHexString()); //tbox
        }
    });

    //For changing the color of chart card appearance
    $(".ChartAppearanceColor").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /*color: 'blanchedalmond',*/
        color: cardColor,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            cardColor = color.toHexString();
            $('#sticky' + id).css("background-color", cardColor);
            //$('#sticky' + id).css("background-color", color.toHexString());
        }
    });

    //For changing the color of table pagination
    $(".TablePaginationColorPicker").spectrum({

        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        /*color: 'blanchedalmond',*/
        color: '#fff',
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        move: function (color) {
            //var table = $('#' + tableName).DataTable();
            //var paginationDiv = '#mytable' + id + '_paginate';
            //table.$(paginationDiv + ' .page-link').css("color", color.toHexString());
            //table.$(paginationDiv + ' .page-link:hover').css("color", color.toHexString());
            //table.$(paginationDiv + ' .page-item.active .page-link').css("background-color", color.toHexString());
            //table.$(paginationDiv + ' .page-item.active .page-link').css("border-color", color.toHexString());
            //table.$(paginationDiv + ' .page-item.active .page-link').css("color", "#fff");

            var paginationDiv = '#mytable' + id + '_paginate';
            $(paginationDiv + ' .page-link').css('color', color.toHexString() + ' !important');
            $(paginationDiv + ' .page-link:hover').css('color', color.toHexString() + ' !important');
            $(paginationDiv + ' .page-item.active .page-link').css('background-color', color.toHexString() + ' !important');
            $(paginationDiv + ' .page-item.active .page-link').css('border-color', color.toHexString() + ' !important');
            $(paginationDiv + ' .page-item.active .page-link').css('color', '#fff !important');

            //paginate_button click event
            $(paginationDiv + ' .paginate_button').click(function () {
                //alert('hello1');
                $(paginationDiv + ' .page-link').css('color', color.toHexString() + ' !important');
                $(paginationDiv + ' .page-link:hover').css('color', color.toHexString() + ' !important');
                $(paginationDiv + ' .page-item.active .page-link').css('background-color', color.toHexString() + ' !important');
                $(paginationDiv + ' .page-item.active .page-link').css('border-color', color.toHexString() + ' !important');
                $(paginationDiv + ' .page-item.active .page-link').css('color', '#fff !important');
            });

        }
    });

    //For changing table title on textbox change
    $('#TableName' + id).keyup(function () {
        // alert('hello');
        var data = document.getElementById('TableName' + stickyID).value;
        headingText = data;
        var iconClass = $('#mainHeader' + id).find('i').attr('class');
        $('#mainHeader' + id).text('');
        if (iconClass == undefined) {
            $('#mainHeader' + id).append(data);
        }
        else {
            var btn = document.createElement("i");
            btn.setAttribute("class", iconClass);
            $('#mainHeader' + id).append(btn);
            $('#mainHeader' + id).append(data);
        }
    });

    //For changing X Axis label on textbox change
    $('#Xaxis' + id).keyup(function () {
        var Xdata = document.getElementById('Xaxis' + stickyID).value;
        x_titlename = Xdata;
        x_showtitle = Boolean(true);
        checkCheckedButton();
        var chartElement = 'myChart' + id;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    });

    //For changing Y Axis label on textbox change
    $('#Yaxis' + id).keyup(function () {
        var Ydata = document.getElementById('Yaxis' + stickyID).value;
        y_titlename = Ydata;
        y_showtitle = Boolean(true);
        checkCheckedButton();
        var chartElement = 'myChart' + id;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    });

    //for heading setting button event
    $('.settingTag').click(function () {
        var getDivId = $(this).attr('id');

        var newDivName = '';
        var newcolorDivName = '';
        var divStatus = '';
        var fontFamilyDiv = '';

        if (getDivId.toLowerCase() == 'tabletitlesettings' + id) {
            newDivName = 'TableTitleFontFamilyDiv' + id;
            newcolorDivName = 'TableTitleColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);
            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');
            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                //$('#' + newDivName).html('');
                $('#' + newDivName).css('display', 'block');
            }
            //headingFontSize = headingFontSize + 'px';
            $("#FontFamilyTableTitleFontFamilyDiv" + id + " > [value='" + headingFontName + "']").attr("selected", "true");
            $("#FontListTableTitleFontFamilyDiv" + id + " > [value='" + headingFontSize + "']").attr("selected", "true");
        }
        else if (getDivId.toLowerCase() == 'tableheadersettings' + id) {
            newDivName = 'TableHeaderFontFamilyDiv' + id;
            newcolorDivName = 'TableHeaderColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);

            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');

            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                //$('#' + newDivName).html('');
                $('#' + newDivName).css('display', 'block');
            }
            $("#FontFamilyTableHeaderFontFamilyDiv" + id + " > [value='" + TableHeaderFontName + "']").attr("selected", "true");
            $("#FontListTableHeaderFontFamilyDiv" + id + " > [value='" + TableHeaderFontSize + "']").attr("selected", "true");
        }
        else if (getDivId.toLowerCase() == 'tablecontentsettings' + id) {
            newDivName = 'TableContentFontFamilyDiv' + id;
            newcolorDivName = 'LabelColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);

            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');

            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                //$('#' + newDivName).html('');
                $('#' + newDivName).css('display', 'block');
            }
            $("#FontFamilyTableContentFontFamilyDiv" + id + " > [value='" + TableCellFontName + "']").attr("selected", "true");
            $("#FontListTableContentFontFamilyDiv" + id + " > [value='" + TableCellFontSize + "']").attr("selected", "true");
        }
        else if (getDivId.toLowerCase() == 'chartlegendsettings' + id) {
            newDivName = 'ChartLegendFontFamilyDiv' + id;
            newcolorDivName = 'LegendColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);

            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');

            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                //$('#' + newDivName).html('');
                $('#' + newDivName).css('display', 'block');
            }
            //legendFontSize = legendFontSize + 'px';
            $("#FontFamilyChartLegendFontFamilyDiv" + id + " > [value='" + legendFontName + "']").attr("selected", "true");
            $("#FontListChartLegendFontFamilyDiv" + id + " > [value='" + legendFontSize + "']").attr("selected", "true");
        }
        else if (getDivId.toLowerCase() == 'chartxaxissettings' + id) {
            newDivName = 'ChartxAxisFontFamilyDiv' + id;
            newcolorDivName = 'ChartxAxisColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);
            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');
            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                //$('#' + newDivName).html('');
                $('#' + newDivName).css('display', 'block');
            }
            //x_fontsize = x_fontsize + 'px';
            $("#FontFamilyChartxAxisFontFamilyDiv" + id + " > [value='" + x_fontfamily + "']").attr("selected", "true");
            $("#FontListChartxAxisFontFamilyDiv" + id + " > [value='" + x_fontsize + "']").attr("selected", "true");
        }
        else if (getDivId.toLowerCase() == 'chartyaxissettings' + id) {
            newDivName = 'ChartyAxisFontFamilyDiv' + id;
            newcolorDivName = 'ChartyAxisColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);
            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');
            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                //$('#' + newDivName).html('');
                $('#' + newDivName).css('display', 'block');
            }
            //y_fontsize = y_fontsize + 'px';
            $("#FontFamilyChartyAxisFontFamilyDiv" + id + " > [value='" + y_fontfamily + "']").attr("selected", "true");
            $("#FontListChartyAxisFontFamilyDiv" + id + " > [value='" + y_fontsize + "']").attr("selected", "true");
        }
        else if (getDivId.toLowerCase() == 'chartxaxislabelsettings' + id) {
            newDivName = 'ChartxAxisLabelFontFamilyDiv' + id;
            newcolorDivName = 'ChartxAxisLabelColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);
            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');
            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                $('#' + newDivName).css('display', 'block');
            }
            //x_label_fontsize = x_label_fontsize + 'px';
            $("#FontFamilyChartxAxisLabelFontFamilyDiv" + id + " > [value='" + x_label_fontfamily + "']").attr("selected", "true");
            $("#FontListChartxAxisLabelFontFamilyDiv" + id + " > [value='" + x_label_fontsize + "']").attr("selected", "true");
        }
        else if (getDivId.toLowerCase() == 'chartyaxislabelsettings' + id) {
            newDivName = 'ChartyAxisLabelFontFamilyDiv' + id;
            newcolorDivName = 'ChartyAxisLabelColorPicker';
            divStatus = $('#' + newDivName).html();
            fontFamilyDiv = fontfamilyDivItem(newDivName, newcolorDivName);
            if (divStatus.toLowerCase() == '') {
                $('#' + newDivName).append(fontFamilyDiv);
                $('#' + newDivName).css('display', 'block');
            }
            else if ($('#' + newDivName).css('display') == 'block') {
                $('#' + newDivName).css('display', 'none');
            }
            else if ($('#' + newDivName).css('display') == 'none') {
                $('#' + newDivName).css('display', 'block');
            }
            else {
                $('#' + newDivName).css('display', 'block');
            }
            //y_label_fontsize = y_label_fontsize + 'px';
            $("#FontFamilyChartyAxisLabelFontFamilyDiv" + id + " > [value='" + y_label_fontfamily + "']").attr("selected", "true");
            $("#FontListChartyAxisLabelFontFamilyDiv" + id + " > [value='" + y_label_fontsize + "']").attr("selected", "true");
        }
        else { }

        //For changing the color of table title
        $(".TableTitleColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /*color: 'blanchedalmond',*/
            color: headingColor,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                headingColor = color.toHexString();
                $('#mainHeader' + id).css("color", headingColor);
                //$('#mainHeader' + id).css("color", color.toHexString());
            }
        });

        //For changing the color of table header
        $(".TableHeaderColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /* color: '#fff',*/
            color: TableHeaderFontColor,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                TableHeaderFontColor = color.toHexString();
                $('#' + tableName + ' thead tr th').css("color", TableHeaderFontColor);
            }
        });

        //For changing the color of table row
        $(".LabelColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /*color: 'blanchedalmond',*/
            color: TableCellFontColor,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                //$('#' + tableName + ' tbody tr td').css("color", color.toHexString());
                //$('#' + tableName).DataTable({
                //    destroy: true,
                //    paging: true,
                //    searching: false,
                //    ordering: true,
                //    info: false,
                //    lengthChange: false,
                //    rowCallback: function (row, data, index) {
                //        $('td', row).css('color', color.toHexString());
                //    }
                //});
                TableCellFontColor = color.toHexString();
                var table = $('#' + tableName).DataTable();
                table.$('td').css('color', TableCellFontColor);
            }
        });

        //For changing the color of legend
        $(".LegendColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /*color: 'blanchedalmond',*/
            color: legendColor,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                legendColor = color.toHexString();
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
        });

        //For changing the color of x axis
        $(".ChartxAxisColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /*color: 'blanchedalmond',*/
            color: x_color,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                x_color = color.toHexString();
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
        });

        //For changing the color of y axis
        $(".ChartyAxisColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /*color: 'blanchedalmond',*/
            color: y_color,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                y_color = color.toHexString();
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
        });

        //For changing the color of x-axis label
        $(".ChartxAxisLabelColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /*color: 'blanchedalmond',*/
            color: x_label_color,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                x_label_color = color.toHexString();
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);


            }
        });

        //For changing the color of y-axis label
        $(".ChartyAxisLabelColorPicker").spectrum({

            showPaletteOnly: true,
            togglePaletteOnly: true,
            togglePaletteMoreText: 'more',
            togglePaletteLessText: 'less',
            /*color: 'blanchedalmond',*/
            color: y_label_color,
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "# ", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ],
            move: function (color) {
                y_label_color = color.toHexString();
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                // updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
        });

        //Click event for bold heading button
        $('#Bold' + newDivName + '').click(function () {
            if (newDivName.toLowerCase() == 'tabletitlefontfamilydiv' + id) {
                var boldStyle = document.getElementById("mainHeader" + id).style.fontWeight;
                if (boldStyle.toLowerCase() != 'bold') {
                    //$('#mainHeader' + id).css("font-weight", "bold !important");
                    headingBold = 'bold';
                    document.getElementById("mainHeader" + id).style.fontWeight = "bold";
                }
                else {
                    //$('#mainHeader' + id).css("font-weight", "normal !important");
                    headingBold = 'normal';
                    document.getElementById("mainHeader" + id).style.fontWeight = "normal";
                }

            }
            else if (newDivName.toLowerCase() == 'tableheaderfontfamilydiv' + id) {
                //var boldStyle = $('#' + tableName + ' thead tr th').style.fontWeight;
                var boldStyle = $('#' + tableName + ' thead tr th').css('font-weight');
                if (boldStyle.toLowerCase() != '700') {
                    TableHeaderBold = 'bold';
                    $('#' + tableName + ' thead tr th').css("font-weight", "700");
                    //$('#' + tableName + ' thead tr th').style.fontWeight = "bold";
                }
                else {
                    TableHeaderBold = 'normal';
                    $('#' + tableName + ' thead tr th').css("font-weight", "400");
                    //$('#' + tableName + ' thead tr th').style.fontWeight = "normal";
                }
            }
            else if (newDivName.toLowerCase() == 'tablecontentfontfamilydiv' + id) {
                var boldStyle = $('#' + tableName + ' tbody tr td').css("font-weight");
                if (boldStyle.toLowerCase() != '700') {
                    TableCellBold = 'bold';
                    var table = $('#' + tableName).DataTable();
                    table.$('td').css("font-weight", "700");
                }
                else {
                    TableCellBold = 'normal';
                    var table = $('#' + tableName).DataTable();
                    table.$('td').css("font-weight", "400");
                }
            }
            else if (newDivName.toLowerCase() == 'chartlegendfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (legendStyle != 'bold') {
                    legendStyle = 'bold';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    legendStyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartxaxisfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (x_fontstyle != 'bold') {
                    x_fontstyle = 'bold';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    x_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartyaxisfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (y_fontstyle != 'bold') {
                    y_fontstyle = 'bold';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    y_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartxaxislabelfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (x_label_fontstyle != 'bold') {
                    x_label_fontstyle = 'bold';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    x_label_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartyaxislabelfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (y_label_fontstyle != 'bold') {
                    y_label_fontstyle = 'bold';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    y_label_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else { }
        });

        //Click event for italic heading button
        $('#Italic' + newDivName + '').click(function () {
            if (newDivName.toLowerCase() == 'tabletitlefontfamilydiv' + id) {
                var italicStyle = document.getElementById("mainHeader" + id).style.fontStyle;
                if (italicStyle.toLowerCase() != 'italic') {
                    headingItalic = 'italic';
                    document.getElementById("mainHeader" + id).style.fontStyle = "italic";
                }
                else {
                    headingItalic = 'normal';
                    document.getElementById("mainHeader" + id).style.fontStyle = "";
                }

            }
            else if (newDivName.toLowerCase() == 'tableheaderfontfamilydiv' + id) {
                var italicStyle = $('#' + tableName + ' thead tr th').css("font-style");
                //var italicStyle = $('#' + tableName + ' thead tr th').style.fontStyle;
                if (italicStyle.toLowerCase() != 'italic') {
                    TableHeaderItalic = 'italic';
                    $('#' + tableName + ' thead tr th').css("font-style", "italic");
                    //$('#' + tableName + ' thead tr th').style.fontStyle = "italic";
                }
                else {
                    TableHeaderItalic = 'normal';
                    $('#' + tableName + ' thead tr th').css("font-style", "");
                    //$('#' + tableName + ' thead tr th').style.fontStyle = "";
                }
            }
            else if (newDivName.toLowerCase() == 'tablecontentfontfamilydiv' + id) {
                var italicStyle = $('#' + tableName + ' tbody tr td').css("font-style");
                if (italicStyle.toLowerCase() != 'italic') {
                    TableCellItalic = 'italic';
                    var table = $('#' + tableName).DataTable();
                    table.$('td').css("font-style", "italic");
                }
                else {
                    TableCellItalic = 'normal';
                    var table = $('#' + tableName).DataTable();
                    table.$('td').css("font-style", "");
                }
            }
            else if (newDivName.toLowerCase() == 'chartlegendfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (legendStyle != 'italic') {
                    legendStyle = 'italic';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    legendStyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartxaxisfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (x_fontstyle != 'italic') {
                    x_fontstyle = 'italic';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    x_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartyaxisfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (y_fontstyle != 'italic') {
                    y_fontstyle = 'italic';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    y_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartxaxislabelfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (x_label_fontstyle != 'italic') {
                    x_label_fontstyle = 'italic';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    x_label_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else if (newDivName.toLowerCase() == 'chartyaxislabelfontfamilydiv' + id) {
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                if (y_label_fontstyle != 'italic') {
                    y_label_fontstyle = 'italic';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
                else {
                    y_label_fontstyle = 'normal';
                    //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                    updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

                }
            }
            else { }
        });

        //Click event for underline heading button
        $('#Underline' + newDivName + '').click(function () {

            if (newDivName.toLowerCase() == 'tabletitlefontfamilydiv' + id) {
                var underlineStyle = document.getElementById("mainHeader" + id).style.textDecoration;
                if (underlineStyle.toLowerCase() != 'underline') {
                    headingUnderline = 'underline';
                    document.getElementById("mainHeader" + id).style.textDecoration = "underline";
                }
                else {
                    headingUnderline = 'normal';
                    document.getElementById("mainHeader" + id).style.textDecoration = "";
                }
            }
            else if (newDivName.toLowerCase() == 'tableheaderfontfamilydiv' + id) {
                var underlineStyle = $('#' + tableName + ' thead tr th').css("text-decoration");
                underlineStyle = underlineStyle.slice(0, underlineStyle.indexOf(' '))
                if (underlineStyle.toLowerCase() != 'underline') {
                    TableHeaderUnderline = 'underline';
                    $('#' + tableName + ' thead tr th').css("text-decoration", "underline");
                }
                else {
                    TableHeaderUnderline = 'normal';
                    $('#' + tableName + ' thead tr th').css("text-decoration", "none");
                }
            }
            else if (newDivName.toLowerCase() == 'tablecontentfontfamilydiv' + id) {
                var underlineStyle = $('#' + tableName + ' tbody tr td').css("text-decoration");
                underlineStyle = underlineStyle.slice(0, underlineStyle.indexOf(' '))
                if (underlineStyle.toLowerCase() != 'underline') {
                    TableCellUnderline = 'underline';
                    var table = $('#' + tableName).DataTable();
                    table.$('td').css("text-decoration", "underline");
                }
                else {
                    TableCellUnderline = 'normal';
                    var table = $('#' + tableName).DataTable();
                    table.$('td').css("text-decoration", "");
                }
            }
            else { }
        });

        //Set font family according to dropdown list item
        $("#FontFamily" + newDivName).change(function () {
            var fontFamily = $(this).val();
            if (newDivName.toLowerCase() == 'tabletitlefontfamilydiv' + id) {
                headingFontName = fontFamily;
                $('#mainHeader' + id).css("fontFamily", headingFontName);
            }
            else if (newDivName.toLowerCase() == 'tableheaderfontfamilydiv' + id) {
                TableHeaderFontName = fontFamily;
                $('#' + tableName + ' thead tr th').css("fontFamily", TableHeaderFontName);
            }
            else if (newDivName.toLowerCase() == 'tablecontentfontfamilydiv' + id) {
                TableCellFontName = fontFamily;
                var table = $('#' + tableName).DataTable();
                table.$('td').css("fontFamily", TableCellFontName);
            }
            else if (newDivName.toLowerCase() == 'chartlegendfontfamilydiv' + id) {
                legendFontName = fontFamily;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartxaxisfontfamilydiv' + id) {
                x_fontfamily = fontFamily;
                x_showtitle = Boolean(true);
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartyaxisfontfamilydiv' + id) {
                y_fontfamily = fontFamily;
                y_showtitle = Boolean(true);
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartxaxislabelfontfamilydiv' + id) {
                x_label_fontfamily = fontFamily;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartyaxislabelfontfamilydiv' + id) {
                y_label_fontfamily = fontFamily;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else { }
        });

        //Set font size according to dropdown list item
        $("#FontList" + newDivName).change(function () {
            var labelNum = $(this).val();
            if (newDivName.toLowerCase() == 'tabletitlefontfamilydiv' + id) {
                headingFontSize = labelNum;
                $('#mainHeader' + id).css("font-size", labelNum);
            }
            else if (newDivName.toLowerCase() == 'tableheaderfontfamilydiv' + id) {
                TableHeaderFontSize = labelNum;
                $('#' + tableName + ' thead tr th').css("font-size", TableHeaderFontSize);
            }
            else if (newDivName.toLowerCase() == 'tablecontentfontfamilydiv' + id) {
                TableCellFontSize = labelNum;
                var table = $('#' + tableName).DataTable();
                table.$('td').css('font-size', TableCellFontSize);
            }
            else if (newDivName.toLowerCase() == 'chartlegendfontfamilydiv' + id) {
                legendFontSize = labelNum;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartxaxisfontfamilydiv' + id) {
                x_fontsize = labelNum;
                x_showtitle = Boolean(true);
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartyaxisfontfamilydiv' + id) {
                y_fontsize = labelNum;
                y_showtitle = Boolean(true);
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartxaxislabelfontfamilydiv' + id) {
                x_label_fontsize = labelNum;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartyaxislabelfontfamilydiv' + id) {
                y_label_fontsize = labelNum;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else { }
        });

        //for alignment heading button event
        $('#Alignment' + newDivName + ' a').on('click', function () {
            var state = $(this).attr('data-val');
            if (newDivName.toLowerCase() == 'tabletitlefontfamilydiv' + id) {
                headingAlign = state;
                $('#mainHeader' + id).css("text-align", state);
            }
            else if (newDivName.toLowerCase() == 'tableheaderfontfamilydiv' + id) {
                TableHeaderAlign = state;
                var column_index = $('#columnList' + id).find('option:selected').val();
                column_index = parseInt(column_index) + 1;
                $('#' + tableName + ' thead tr th:nth-child' + '(' + column_index + ')').css("text-align", TableHeaderAlign);
            }
            else if (newDivName.toLowerCase() == 'tablecontentfontfamilydiv' + id) {
                TableCellAlign = state;
                var columnIndex = $('#ContentcolumnList' + id).find('option:selected').val();
                columnIndex = parseInt(columnIndex) + 1;
                var table = $('#' + tableName).DataTable();
                table.$('td:nth-child' + '(' + columnIndex + ')').css("text-align", TableCellAlign);
            }
            else if (newDivName.toLowerCase() == 'chartlegendfontfamilydiv' + id) {
                legendAlign = state;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartxaxisfontfamilydiv' + id) {
                x_align = state;
                x_showtitle = Boolean(true);
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartyaxisfontfamilydiv' + id) {
                y_align = state;
                y_showtitle = Boolean(true);
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartxaxislabelfontfamilydiv' + id) {
                x_label_align = state;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else if (newDivName.toLowerCase() == 'chartyaxislabelfontfamilydiv' + id) {
                y_label_align = state;
                checkCheckedButton();
                var chart_ele = 'myChart' + id;
                //updateLegends(chart_ele, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
                updateLegends(chart_ele, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

            }
            else { }
        });

        //disable underline and alignment button of legend and axis menu
        $('#UnderlineChartLegendFontFamilyDiv' + id).attr('disabled', true);
        $('#UnderlineChartxAxisFontFamilyDiv' + id).attr('disabled', true);
        $('#UnderlineChartyAxisFontFamilyDiv' + id).attr('disabled', true);
        $('#UnderlineChartxAxisLabelFontFamilyDiv' + id).attr('disabled', true);
        $('#UnderlineChartyAxisLabelFontFamilyDiv' + id).attr('disabled', true);
        $('#dropdownMenuTitleChartxAxisFontFamilyDiv' + id).attr('disabled', true);
        $('#dropdownMenuTitleChartyAxisFontFamilyDiv' + id).attr('disabled', true);
        $('#dropdownMenuTitleChartxAxisLabelFontFamilyDiv' + id).attr('disabled', true);
        $('#dropdownMenuTitleChartyAxisLabelFontFamilyDiv' + id).attr('disabled', true);
    });

}

//For apply border shadow effect on table
$(document).on('click', '#TableBorderShadow', function () {
    if ($(this).is(':checked')) {
        showLayoutBorderShadow = Boolean(true);
        $('#sticky' + stickyID).css("boxShadow", "4px 4px #888888");
    } else {
        showLayoutBorderShadow = Boolean(false);
        $('#sticky' + stickyID).css("boxShadow", "none");
    }
});

//For apply border shadow effect on table header
$(document).on('click', '#TableHeaderBorderShadow', function () {
    if ($(this).is(':checked')) {
        showHeaderBorderShadow = Boolean(true);
        $('#mytable' + stickyID + ' thead tr th').css("border-bottom", "3px solid #dee2e6");
    } else {
        showHeaderBorderShadow = Boolean(false);
        $('#mytable' + stickyID + ' thead tr th').css("border-bottom", "2px solid #dee2e6");
    }

});

//For hide and show header
$(document).on('click', '#ShowTableHeader', function () {
    if ($(this).is(':checked')) {
        showHeader = Boolean(true);
        $('#mytable' + stickyID + ' thead').css("display", "table-header-group");
    } else {
        showHeader = Boolean(false);
        $('#mytable' + stickyID + ' thead').css("display", "none");
    }

});

//For hide and show table pagination //DATATABLE//
$(document).on('click', '#ShowTablePagination', function () {
    if ($(this).is(':checked')) {
        //$('#mytable' + stickyID + '_paginate').css("display", "block");
        ShowPagination = Boolean(true);
        $('#mytable' + stickyID).DataTable({
            destroy: true,
            paging: true,
            searching: false,
            ordering: true,
            info: false,
            lengthChange: false,
            language: {
                oPaginate: {
                    sNext: '<i class="fa fa-forward"></i>',
                    sPrevious: '<i class="fa fa-backward"></i>'

                }
            }
        });
    } else {
        //$('#mytable' + stickyID + '_paginate').css("display", "none");
        ShowPagination = Boolean(false);
        $('#mytable' + stickyID).DataTable({
            destroy: true,
            paging: false,
            searching: false,
            ordering: true,
            info: false,
            lengthChange: false
        });
    }

});

//For compact format of table row
$(document).on('click', '#TableCompactFormat', function () {
    var columnIndex = $('#ContentcolumnList' + stickyID).find('option:selected').val();
    columnIndex = parseInt(columnIndex);
    var colData = [];
    colData.push(columnIndex);
    if ($(this).is(':checked')) {
        $('#mytable' + stickyID).DataTable({
            destroy: true,
            paging: true,
            searching: false,
            ordering: true,
            info: false,
            lengthChange: false,
            columnDefs: [{
                /* "targets": [2],*/
                /* "targets": '['+columnIndex+']',*/
                "targets": colData,
                "render": function (data, type, row) {
                    numericData.push(data);
                    //console.log("datanew:", data);
                    //return Number(data).toLocaleString('en-IN', {
                    //    maximumFractionDigits: 2
                    //    //style: 'currency',
                    //    //currency: 'INR'
                    //});

                    var nData = Number(data).toLocaleString('en-IN', {
                        maximumFractionDigits: 2
                        //style: 'currency',
                        //currency: 'INR'
                    });

                    if (nData == null) {
                        return data;
                    }
                    else return nData;

                    //return data.toString().match(/\d+(\.\d{1,2})?/g)[0];
                    //return parseFloat(data).toFixed(2);

                }
            }]
        });
    }
    else {
        $('#mytable' + stickyID).DataTable({
            destroy: true,
            paging: true,
            searching: false,
            ordering: true,
            info: false,
            lengthChange: false,
            columnDefs:
                [{
                    targets: colData,
                    data: numericData
                }]
            //columnDefs: [{
            //    "targets": colData,
            //    "render": function (data, type, row) {
            //        //data = numericData;
            //        console.log("numericData:", numericData);
            //        return data;
            //        //console.log("data:", data);
            //        //for (let i = 0; i < numericData.length; i++) {
            //        //    data[i]= numericData[i];
            //        //}
            //        //console.log("datafinal:", data);
            //        //return commaSeparateNumber(numericData);
            //    }
            //}]
        });
    }

});

//For hide and show table icon
$(document).on('click', '#IconShow', function () {
    if ($(this).is(':checked')) {
        IconShow = Boolean(true);
        //$('#mainHeader' + stickyID).find('i').css("display", "none");
        var titlename = $('#mainHeader' + stickyID).text();
        $('#mainHeader' + stickyID).text('');
        var btn = document.createElement("i");
        btn.setAttribute("class", "fa fa-th mr-1");
        $('#mainHeader' + stickyID).append(btn);
        $('#mainHeader' + stickyID).append(titlename);
        $('#mainHeader' + stickyID).find('i').css("display", "");

    } else {
        IconShow = Boolean(false);
        //$('#mainHeader' + stickyID).find('i').css("display", "");
        $('#mainHeader' + stickyID).find('i').remove();
    }

});

//For hide and show table icon
$(document).on('click', '#TitleShow', function () {
    if ($(this).is(':checked')) {
        $('#mainHeader' + stickyID).remove();
        $('#TableName' + stickyID).val('');

    } else {
        var newDivHeader = '<h3 class="card-title col-11" id="mainHeader' + stickyID + '">' + StickyTablename + '</h3>';
        $('.card-header').append(newDivHeader);
        $('#TableName' + stickyID).val(StickyTablename);
    }

});

//For hide and show chart legend
$(document).on('click', '#showchartlegend', function () {
    var chartElement;
    var AxisCheck = $('#AxisShow').is(':checked');
    if (AxisCheck == true) {
        AxisShow = Boolean(true);
    }
    else {
        AxisShow = Boolean(false);
    }
    var GridlineCheck = $('#GridlinesShow').is(':checked');
    if (GridlineCheck == true) {
        gridLinesShow = Boolean(true);
    }
    else {
        gridLinesShow = Boolean(false);
    }
    //checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align);
    checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, y_label_fontsize, x_label_fontsize, y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    if ($(this).is(':checked')) {
        legendShow = Boolean(true);
        //var val = 'fa fa-eye';
        chartElement = 'myChart' + stickyID;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    }
    else {
        legendShow = Boolean(false);
        // var val = 'fa fa-eye-slash';
        chartElement = 'myChart' + stickyID;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    }
});

//For hide and show chart axis
$(document).on('click', '#AxisShow', function () {
    var chartElement;
    var legendCheck = $('#showchartlegend').is(':checked');
    if (legendCheck == false) {
        legendShow = Boolean(false);
    }
    else {
        legendShow = Boolean(true);
    }
    var GridlineCheck = $('#GridlinesShow').is(':checked');
    if (GridlineCheck == true) {
        gridLinesShow = Boolean(true);
    }
    else {
        gridLinesShow = Boolean(false);
    }

    //checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align);
    checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, y_label_fontsize, x_label_fontsize, y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    if ($(this).is(':checked')) {
        AxisShow = Boolean(true);
        chartElement = 'myChart' + stickyID;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    }
    else {
        AxisShow = Boolean(false);
        chartElement = 'myChart' + stickyID;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    }
});

//For hide and show chart gridlines
$(document).on('click', '#GridlinesShow', function () {
    var chartElement;
    var legendCheck = $('#showchartlegend').is(':checked');
    if (legendCheck == false) {
        legendShow = Boolean(false);
    }
    else {
        legendShow = Boolean(true);
    }
    var AxisCheck = $('#AxisShow').is(':checked');
    if (AxisCheck == true) {
        AxisShow = Boolean(true);
    }
    else {
        AxisShow = Boolean(false);
    }

    //checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align);
    checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, y_label_fontsize, x_label_fontsize, y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
    if ($(this).is(':checked')) {
        gridLinesShow = Boolean(true);
        chartElement = 'myChart' + stickyID;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    }
    else {
        gridLinesShow = Boolean(false);
        chartElement = 'myChart' + stickyID;
        //updateLegends(chartElement, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
        updateLegends(chartElement, chartTypeName, legendColor, parseInt(legendFontSize), legendFontName, legendAlign, legendPosition, legendStyle, legendShow, parseInt(xAxisFontSize), parseInt(yAxisFontSize), AxisShow, gridLinesShow, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, parseInt(y_fontsize), y_fontstyle, y_align, x_color, x_fontfamily, parseInt(x_fontsize), x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, parseInt(y_label_fontsize), parseInt(x_label_fontsize), y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);

    }
});

//function for radio button checked
function handleClick(myRadio) {
    var getId = myRadio.value.split(/(\d+)/);
    var e = document.getElementById("columnList" + getId[1]);
    var columnName = e.value;
    var selectId = e.id;
    var columnType = $('#' + selectId).find('option:selected').attr('data-id');

    //if (columnType.toLowerCase() == "string") {
    //    sortTable(myRadio.name, columnName, getId[0])
    //}
    //else if (columnType.toLowerCase() == "number") {
    //    sortNumericTable(myRadio.name, columnName, getId[0])
    //}
    //else { }

    $('#mytable' + stickyID).DataTable({
        destroy: true,
        paging: true,
        searching: false,
        /*ordering: false,*/
        info: false,
        lengthChange: false,
        order: [[columnName, getId[0]]]
    });


}

//For closing the sidebar
function closeNav(id) {
    //$('#propertyRef' + id).remove();
    $(".propertyBar").css('display', 'none');
    $('#mySidebar').html('');
    json_DataProperty = getUniqueListBy(json_DataProperty, 'key');
    getDivJsonData(chartTypeName);
    property_values(id);
    //reinitializeVariable();
}

function closeBtn() {
    $(".propertyBar").css('display', 'none');
    $('#mySidebar').html('');
    json_DataProperty = getUniqueListBy(json_DataProperty, 'key');
    getDivJsonData(chartTypeName);
    //reinitializeVariable();
}

function property_values(sticky_id) {
    var tmp_data = json_DataProperty;
    //console.log("TEMPDATA:", tmp_data);
    if (tmp_data.length != 0) {
        var tmp_obj = tmp_data.find(o => o.key === 'sticky' + sticky_id);
        if (tmp_obj != undefined) {
            current_JSONDataProperty = tmp_obj.value;
            //console.log("current_JSONDataProperty:", current_JSONDataProperty);
            if (chartTypeName.toLocaleLowerCase() == 'tabular') { tableValueInitialize(); }
            else if (chartTypeName.toLocaleLowerCase() == 'label') { }
            else {
                chartValueInitialize();
            }
        }
    }
}

function del_property_values(sticky_id) {
    var keyName = 'sticky' + sticky_id;
    var filterData = json_DataProperty.filter((item) => item.key !== keyName);
    json_DataProperty = filterData;
}

function edit_property_values(sticky_id, sticky_type) {
    var tmp_data = json_DataProperty;
    //console.log("TEMPDATA:", tmp_data);
    if (tmp_data.length != 0) {
        var tmp_obj = tmp_data.find(o => o.key === 'sticky' + sticky_id);
        if (tmp_obj != undefined) {
            current_JSONDataProperty = tmp_obj.value;
            //console.log("current_JSONDataProperty:", current_JSONDataProperty);
            if (sticky_type.toLocaleLowerCase() == 'tabular') { tableValueInitialize(); }
            else if (sticky_type.toLocaleLowerCase() == 'label') { }
            else {
                chartValueInitialize();
            }
        }
    }
}

//Create common custom setting div for styling
function fontfamilyDivItem(div_name, color_name) {
    var itemDiv = '';

    itemDiv = '<div class="row">'
        + '<div class="col-sm-2 mt-2">'
        + '<input type = "text" class="' + color_name + '" name="' + color_name + '"/>'
        + '</div>'
        + '<div class="col-sm-6 mt-2">'
        + '<select name="HeaderFontFamily" class="org form-control mb-1" id="FontFamily' + div_name + '">'
        + '<option value="Arial" style="font-family:Arial;"> Arial </option>'
        + '<option value="Brush Script MT" style="font-family:Brush Script MT;"> Brush Script MT </option>'
        + '<option value="Courier New" style="font-family:Courier New;"> Courier New </option>'
        + '<option value="Copperplate" style="font-family:Copperplate;"> Copperplate </option>'
        + '<option value="Helvetica" style="font-family:Helvetica;"> Helvetica </option>'
        + '<option value="Georgia" style="font-family:Georgia;"> Georgia </option>'
        + '<option value="Impact" style="font-family:Impact;"> Impact </option>'
        + '<option value="Lucida Console" style="font-family:Lucida Console;"> Lucida Console </option>'
        + '<option value="Tahoma" style="font-family:Tahoma;"> Tahoma </option>'
        + '<option value="Times New Roman" style="font-family:Times New Roman;"> Times New Roman </option>'
        + '<option value="Trebuchet MS" style="font-family:Trebuchet MS;"> Trebuchet MS </option>'
        + '<option value="Papyrus" style="font-family:Papyrus;"> Papyrus </option>'
        + '<option value="Verdana" style="font-family:Verdana;"> Verdana </option>'
        + '<option value="Montserrat" style="font-family:Montserrat;"> Montserrat </option>'
        + '</select></div>'
        + '<div class="col-sm-4 mt-2">'
        + '<select name="FontSize" class="org form-control mb-1" id="FontList' + div_name + '">'
        + '<option value="8px"> 8 </option>'
        + '<option value="9px"> 9 </option>'
        + '<option value="10px" selected> 10 </option>'
        + '<option value="12px"> 12 </option>'
        + '<option value="14px"> 14 </option>'
        + '<option value="16px"> 16 </option>'
        + '<option value="18px"> 18 </option>'
        + '<option value="20px"> 20 </option>'
        + '<option value="22px"> 22 </option>'
        + '<option value="24px"> 24 </option>'
        + '<option value="28px"> 28 </option>'
        + '<option value="30px"> 30 </option>'
        + '</select></div>'
        + '</div>'
        + '<div class="row">'
        + '<div class="col-sm-3 mt-2"><button id = "Bold' + div_name + '" class="form-control" data-toggle="tooltip" title = "Bold"><i class="fas fa-bold"></i></button></div>'
        + '<div class="col-sm-3 mt-2"><button id = "Italic' + div_name + '" class="form-control" data-toggle="tooltip" title = "Italic"><i class="fas fa-italic"></i></button></div>'
        + '<div class="col-sm-3 mt-2"><button id = "Underline' + div_name + '" class="form-control" data-toggle="tooltip" title = "Underline"><i class="fas fa-underline"></i></button></div>'
        + '<div class="dropdown col-sm-3 mt-2">'
        //+ '<a type="button" id="dropdownMenu2" class="form-control" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fas fa-bars"></i></a>'
        + '<button id="dropdownMenuTitle' + div_name + '" class="form-control" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" title="Alignment"><i class="fas fa-bars"></i></button>'
        + '<div class="dropdown-menu dropdown-primary" style="min-width: 4rem;" id="Alignment' + div_name + '">'
        + '<a class="dropdown-item" style="text-align:center" data-val="right" title="Right"><i class="fas fa-align-right"></i></a>'
        + '<a class="dropdown-item" style="text-align:center" data-val="center" title="Center"><i class="fas fa-align-center"></i></a>'
        + '<a class="dropdown-item" style="text-align:center" data-val="left" title="Left"><i class="fas fa-align-left"></i></a>'
        + '</div></div>'

    return itemDiv;
}

//Create HTML format for sidebar property window from json data
function generateTableHtml(data, table_id, selectCol, chart_name, table_name) {
    var htmlformat = '';
    var titlename = data[0]['name'];
    var titleheading = data[0]['tableheading'];
    var titleheader = data[0]['tableheader'];
    var titledata = data[0]['tabledata'];
    var titlelayout = data[0]['tablelayout'];
    var titlefooter = data[0]['tablefooter'];
    var chartlegend = data[0]['legend'];
    var chartaxis = data[0]['axis'];
    var chartlabel = data[0]['label'];

    htmlformat += '<a href="javascript: void (0)" id="propertyRef' + table_id + '" class="closebtn" onclick="closeNav(' + table_id + ')">×</a>'
        + '<div id="myChartProperty' + table_id + '" class="propertyCanvas"></div>'
        + '<h3>' + titlename + '</h3>'
        + '<ul class="list-unstyled components mb-2"><hr class="mt-1">';

    if (titleheading != undefined) {
        var heading_array = titleheading.split(',');
        htmlformat += '<li><a href="#tableTitle" data-toggle="collapse" aria-expanded="false" class="dropdown-toggle accordion__headings-title chart-settings__title"><span class="fas fa-heading"></span>  Heading </a>'
            + '<ul class="collapse list-unstyled" id="tableTitle">'
            + '<li>'
        for (var i = 0; i < heading_array.length; i++) {
            heading_array[i] = heading_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (heading_array[i].toLowerCase() == 'title') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-3 mt-2"> Title</div>'
                    + '<div class="col-sm-7">'
                    + '<input id="TableName' + table_id + '" type="text" class="form-control" value="' + chart_name + '" /></div>';
            }
            else if (heading_array[i].toLowerCase() == 'font') {
                htmlformat += '<div class="col-sm-2 mt-2"><i id="TableTitleSettings' + table_id + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="TableTitleFontFamilyDiv' + table_id + '"></div>';
            }
            else if (heading_array[i].toLowerCase() == 'backgroundcolor') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-2 mt-2">'
                    + '<input type = "text" class="ChartAppearanceColor" name="ChartAppearanceColor"/></div>'
                    + '<div class="col-sm-10 mt-3">'
                    + '<h6>Card Background Color</h6>'
                    + '</div></div>';
            }
            else if (heading_array[i].toLowerCase() == 'showicon') {
                htmlformat += '<div class="mb-1 mt-2 ml-2"><input type="checkbox" id="IconShow" class="mr-2"><span>Show icon</span></div>'
            }
            else if (heading_array[i].toLowerCase() == 'removetitle') {
                htmlformat += '<div class="mb-1 mt-2 ml-2"><input type="checkbox" id="TitleShow" class="mr-2"><span>Remove title</span></div>'
            }
            else {
                htmlformat += '';
            }

        }
        htmlformat += '</li></ul></li><hr class="mt-1 mb-1">';
    }
    if (titleheader != undefined) {
        var header_array = titleheader.split(',');
        htmlformat += '<li><a href="#tableHeader" data-toggle="collapse" aria-expanded="false" class="dropdown-toggle"><span class="fas fa-table"></span> Table Header </a>'
            + '<ul class="collapse list-unstyled" id="tableHeader">'
            + '<li>'
        for (var i = 0; i < header_array.length; i++) {
            header_array[i] = header_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (header_array[i].toLowerCase() == 'columnlist') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-3 mt-2">Column</div>'
                    + '<div class="col-sm-7">'
                    + '<select name="tablecolumn" class="org form-control mb-1" id="columnList' + stickyID + '">' + selectCol + '</select></div>';
            }
            else if (header_array[i].toLowerCase() == 'font') {
                htmlformat += '<div class="col-sm-2 mt-2"><i id="TableHeaderSettings' + stickyID + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="TableHeaderFontFamilyDiv' + stickyID + '"></div>';
            }
            else if (header_array[i].toLowerCase() == 'sorting') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-3 mt-2">Sorting</div>'
                    + '<div class="col-sm-4 mt-2 pr-0 pl-0">'
                    + '<input type="radio" name="' + table_name + '" value="asc' + stickyID + '" class="mr-1" checked="checked" onclick="handleClick(this);"><span>Ascending</span></div>'
                    + '<div class="col-sm-4 mt-2 pr-0 pl-0">'
                    + '<input type="radio" name="' + table_name + '" value="desc' + stickyID + '" class="mr-1" onclick="handleClick(this);"><span>Descending</span></div><div class="col-sm-1 mt-2"></div>'
                    + '</div>';
            }
            else if (header_array[i].toLowerCase() == 'backgroundcolor') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-2 mt-2">'
                    + '<input type = "text" class="BackgroundColorPicker" name="BackgroundColorPicker"/></div>'
                    + '<div class="col-sm-10 mt-3">'
                    + '<h6>Header Background Color</h6>'
                    + '</div></div>';
            }
            else if (header_array[i].toLowerCase() == 'bordershadow') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-12 mt-2 ml-2"><input type="checkbox" id="TableHeaderBorderShadow" class="mr-1"><span>Add Border Shadow</span></div></div>';
            }
            else if (header_array[i].toLowerCase() == 'showheader') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-12 mt-2 ml-2"><input type="checkbox" id="ShowTableHeader" checked="checked" class="mr-1"><span>Show Header</span></div></div>';
            }
            else if (header_array[i].toLowerCase() == 'wraptext') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-12 mt-2 ml-2"><input type="checkbox" id="WrapHeaderText" class="mr-1"><span>Wrap Text</span></div></div>';
            }
            else {
                htmlformat += '';
            }
        }
        htmlformat += '</li></ul></li><hr class="mt-1 mb-1">';
    }
    if (titledata != undefined) {
        var data_array = titledata.split(',');
        htmlformat += '<li><a href="#tableContent" data-toggle="collapse" aria-expanded="false" class="dropdown-toggle"><span class="fas fa-font"></span> Table Data </a>'
            + '<ul class="collapse list-unstyled" id="tableContent">'
            + '<li>'
        for (var i = 0; i < data_array.length; i++) {
            data_array[i] = data_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (data_array[i].toLowerCase() == 'columnlist') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-3 mt-2">Column</div>'
                    + '<div class="col-sm-7">'
                    + '<select name="cars" class="org form-control mb-1" id="ContentcolumnList' + stickyID + '">' + selectCol + '</select></div>';
            }
            else if (data_array[i].toLowerCase() == 'font') {
                htmlformat += '<div class="col-sm-2 mt-2"><i id="TableContentSettings' + stickyID + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="TableContentFontFamilyDiv' + stickyID + '"></div>';
            }
            else {
                htmlformat += '';
            }
        }
        htmlformat += '</li></ul></li></ul></div><hr class="mt-1 mb-1">';
    }
    if (titlelayout != undefined) {
        var layout_array = titlelayout.split(',');
        htmlformat += '<h6>Table Layout</h6>'
        for (var i = 0; i < layout_array.length; i++) {
            layout_array[i] = layout_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (layout_array[i].toLowerCase() == 'backgroundcolor') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-2">'
                    + '<input type = "text" class="CardBackgroundColorPicker" name="CardBackgroundColorPicker"/></div>'
                    + '<div class="col-sm-10 mt-1">'
                    + '<h6>Table Background Color</h6>'
                    + '</div></div>';
            }
            else if (layout_array[i].toLowerCase() == 'cellcolor') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-2">'
                    + '<input type = "text" class="CellBorderColorPicker" name="CellBorderColorPicker"/></div>'
                    + '<div class="col-sm-10 mt-1">'
                    + '<h6>Cell Border Color</h6>'
                    + '</div></div>';
            }
            else if (layout_array[i].toLowerCase() == 'oddcellcolor') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-2">'
                    + '<input type = "text" class="OddCellColorPicker" name="OddCellColorPicker"/></div>'
                    + '<div class="col-sm-10 mt-1">'
                    + '<h6>Odd Cell Color</h6>'
                    + '</div></div>';
            }
            else if (layout_array[i].toLowerCase() == 'evencellcolor') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-2">'
                    + '<input type = "text" class="EvenCellColorPicker" name="EvenCellColorPicker"/></div>'
                    + '<div class="col-sm-10 mt-1">'
                    + '<h6>Even Cell Color</h6>'
                    + '</div></div>';
            }
            else if (layout_array[i].toLowerCase() == 'bordershadow') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-12 mt-2 ml-2"><input type="checkbox" id="TableBorderShadow" class="mr-1"><span>Add Border Shadow</span></div></div>';
            }
            else {
                htmlformat += '';
            }
        }
        htmlformat += '<hr>';
    }
    if (titlefooter != undefined) {
        var footer_array = titlefooter.split(',');
        htmlformat += '<h6>Table Footer</h6>'
        for (var i = 0; i < footer_array.length; i++) {
            footer_array[i] = footer_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (footer_array[i].toLowerCase() == 'showpagination') {
                htmlformat += '<div class="mb-1 mt-2 ml-2"><input type="checkbox" id="ShowTablePagination" checked="checked" class="mr-1"><span>Show Pagination</span></div>';
            }
            else if (footer_array[i].toLowerCase() == 'compactnumber') {
                htmlformat += '<div class="mb-1 mt-2 ml-2"><input type="checkbox" id="TableCompactFormat" class="mr-1"><span>Compact Numbers</span></div>';

            }
            else {
                htmlformat += '';
            }
        }
        //htmlformat += '';
    }
    if (chartlegend != undefined) {
        var legend_array = chartlegend.split(',');
        htmlformat += '<li><a href="#chartlegend" data-toggle="collapse" aria-expanded="false" class="dropdown-toggle"><span class="fas fa-th-large"></span> Legend </a>'
            + '<ul class="collapse list-unstyled" id="chartlegend">'
            + '<li>'
        for (var i = 0; i < legend_array.length; i++) {
            legend_array[i] = legend_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (legend_array[i].toLowerCase() == 'position') {
                htmlformat += '<div class="row">'
                    + '<div class="col-sm-3 mt-2">Position</div>'
                    + '<div class="col-sm-7">'
                    + '<select name="cars" class="org form-control" id="LegendPositionList' + stickyID + '">'
                    + '<option value="left"> Left </option>'
                    + '<option value="right"> Right </option>'
                    + '<option value="top"> Top </option>'
                    + '<option value="bottom"> Bottom </option>'
                    + '</select></div>';
            }
            else if (legend_array[i].toLowerCase() == 'font') {
                htmlformat += '<div class="col-sm-2 mt-2"><i id="ChartLegendSettings' + stickyID + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="ChartLegendFontFamilyDiv' + stickyID + '"></div>';
            }
            else if (legend_array[i].toLowerCase() == 'showlegend') {
                htmlformat += '<div class="mb-1 mt-2 ml-2"><input type="checkbox" id="showchartlegend" class="mr-1"><span>Show Legend</span></div>'
            }
            else {
                htmlformat += '';
            }
        }
        htmlformat += '</li></ul></li><hr class="mt-1 mb-1">';

    }
    if (chartaxis != undefined) {
        var axis_array = chartaxis.split(',');
        htmlformat += '<li><a href="#chartxAxis" data-toggle="collapse" aria-expanded="false" class="dropdown-toggle accordion__headings-title chart-settings__title"><span class="far fa-chart-bar"></span>  Axis </a>'
            + '<ul class="collapse list-unstyled" id="chartxAxis">'
            + '<li>'
        for (var i = 0; i < axis_array.length; i++) {
            axis_array[i] = axis_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (axis_array[i].toLowerCase() == 'xaxis') {
                htmlformat += '<div class="row mt-2">'
                    + '<div class="col-sm-3"> X Axis</div>'
                    + '<div class="col-sm-7">'
                    + '<input id="Xaxis' + table_id + '" type="text" class="form-control" placeholder="Default"/></div>';
            }
            else if (axis_array[i].toLowerCase() == 'xfont') {
                htmlformat += '<div class="col-sm-2"><i id="ChartxAxisSettings' + table_id + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="ChartxAxisFontFamilyDiv' + table_id + '"></div>';
            }
            else if (axis_array[i].toLowerCase() == 'yaxis') {
                htmlformat += '<div class="row mt-2">'
                    + '<div class="col-sm-3"> Y Axis</div>'
                    + '<div class="col-sm-7">'
                    + '<input id="Yaxis' + table_id + '" type="text" class="form-control" placeholder="Default"/></div>';
            }
            else if (axis_array[i].toLowerCase() == 'yfont') {
                htmlformat += '<div class="col-sm-2"><i id="ChartyAxisSettings' + table_id + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="ChartyAxisFontFamilyDiv' + table_id + '"></div>';
            }
            else if (axis_array[i].toLowerCase() == 'showaxis') {
                htmlformat += '<div class="mb-1 mt-2 ml-2"><input type="checkbox" id="AxisShow" checked="checked" class="mr-2"><span>Show axis</span></div>'
            }
            else if (axis_array[i].toLowerCase() == 'showgridlines') {
                htmlformat += '<div class="mb-1 mt-2 ml-2"><input type="checkbox" id="GridlinesShow" checked="checked" class="mr-2"><span>Show grid lines</span></div>'
            }
            else {
                htmlformat += '';
            }

        }
        htmlformat += '</li></ul></li><hr class="mt-1 mb-1">';
    }
    if (chartlabel != undefined) {
        var label_array = chartlabel.split(',');
        htmlformat += '<li><a href="#chartLabel" data-toggle="collapse" aria-expanded="false" class="dropdown-toggle accordion__headings-title chart-settings__title"><span class="fab fa-deezer"></span>  Label </a>'
            + '<ul class="collapse list-unstyled" id="chartLabel">'
            + '<li>'
        for (var i = 0; i < label_array.length; i++) {
            label_array[i] = label_array[i].replace(/^\s*/, "").replace(/\s*$/, "");

            if (label_array[i].toLowerCase() == 'xfont') {
                htmlformat += '<div class="row mt-2"><div class="col-sm-10">X Axis</div><div class="col-sm-2"><i id="ChartxAxisLabelSettings' + table_id + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="ChartxAxisLabelFontFamilyDiv' + table_id + '"></div></div>';
            }
            if (label_array[i].toLowerCase() == 'yfont') {
                htmlformat += '<div class="row mt-2"><div class="col-sm-10">Y Axis</div><div class="col-sm-2"><i id="ChartyAxisLabelSettings' + table_id + '" class="settingTag fas fa-cog"></i></div></div>'
                    + '<div id="ChartyAxisLabelFontFamilyDiv' + table_id + '"></div></div>';
            }
            else {
                htmlformat += '';
            }

        }
        htmlformat += '</li></ul></li><hr class="mt-1 mb-1">';
    }

    else {
        htmlformat += '</ul>';
    }

    return htmlformat;

}

function checkValue(font_color, font_size, font_family, label_align, legend_position, legend_style, show_legend, yAxis_fontsize, xAxis_fontsize, show_axis, show_gridlines, Y_showtitle, Y_titlename, X_showtitle, X_titlename, ycolor, yfontfamily, yfontsize, yfontstyle, yalign, xcolor, xfontfamily, xfontsize, xfontstyle, xalign, ylabelcolor, xlabelcolor, ylabelfontfamily, xlabelfontfamily, ylabelfontsize, xlabelfontsize, ylabelfontstyle, xlabelfontstyle, ylabelalign, xlabelalign) {
    if (font_color != '') {
        legendColor = font_color;
    }
    if (font_size != '') {
        //legendFontSize = parseInt(font_size);
        legendFontSize = font_size;
    }
    if (font_family != '') {
        legendFontName = font_family;
    }
    if (label_align != '') {
        legendAlign = label_align;
    }
    if (legend_position != '') {
        legendPosition = legend_position;
    }
    if (legend_style != '') {
        legendStyle = legend_style;
    }
    if (show_legend != false) {
        legendShow = Boolean(true);
    }
    if (yAxis_fontsize != '') {
        //yAxisFontSize = parseInt(yAxis_fontsize);
        yAxisFontSize = yAxis_fontsize;
    }
    if (xAxis_fontsize != '') {
        //xAxisFontSize = parseInt(xAxis_fontsize);
        xAxisFontSize = xAxis_fontsize;
    }
    if (show_axis != false) {
        AxisShow = Boolean(true);
    }
    if (show_gridlines != false) {
        gridLinesShow = Boolean(true);
    }
    if (Y_showtitle != false) {
        y_showtitle = Boolean(true);
    }
    if (Y_titlename != '') {
        y_titlename = Y_titlename;
    }
    if (X_showtitle != false) {
        x_showtitle = Boolean(true);
    }
    if (X_titlename != '') {
        x_titlename = X_titlename;
    }
    if (ycolor != '') {
        y_color = ycolor;
    }
    if (yfontfamily != '') {
        y_fontfamily = yfontfamily;
    }
    if (yfontsize != '') {
        //y_fontsize = parseInt(yfontsize);
        y_fontsize = yfontsize;
    }
    if (yalign != '') {
        y_align = yalign;
    }
    if (yfontstyle != '') {
        y_fontstyle = yfontstyle;
    }
    if (xcolor != '') {
        x_color = xcolor;
    }
    if (xfontfamily != '') {
        x_fontfamily = xfontfamily;
    }
    if (xfontsize != '') {
        //x_fontsize = parseInt(xfontsize);
        x_fontsize = xfontsize;
    }
    if (xalign != '') {
        x_align = xalign;
    }
    if (xfontstyle != '') {
        x_fontstyle = xfontstyle;
    }
    if (ylabelcolor != '') {
        y_label_color = ylabelcolor;
    }
    if (ylabelfontfamily != '') {
        y_label_fontfamily = ylabelfontfamily;
    }
    if (ylabelfontsize != '') {
        //y_label_fontsize = parseInt(ylabelfontsize);
        y_label_fontsize = ylabelfontsize;
    }
    if (ylabelalign != '') {
        y_label_align = ylabelalign;
    }
    if (ylabelfontstyle != '') {
        y_label_fontstyle = ylabelfontstyle;
    }
    if (xlabelcolor != '') {
        x_label_color = xlabelcolor;
    }
    if (xlabelfontfamily != '') {
        x_label_fontfamily = xlabelfontfamily;
    }
    if (xlabelfontsize != '') {
        //x_label_fontsize = parseInt(xlabelfontsize);
        x_label_fontsize = xlabelfontsize;
    }
    if (xlabelalign != '') {
        x_label_align = xlabelalign;
    }
    if (xlabelfontstyle != '') {
        x_label_fontstyle = xlabelfontstyle;
    }
}
function checkCheckedButton() {
    var AxisCheck = $('#AxisShow').is(':checked');
    var GridlineCheck = $('#GridlinesShow').is(':checked');
    var legendCheck = $('#showchartlegend').is(':checked');
    //checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendCheck, xAxisFontSize, yAxisFontSize, AxisCheck, GridlineCheck, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align);
    checkValue(legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendCheck, xAxisFontSize, yAxisFontSize, AxisCheck, GridlineCheck, y_showtitle, y_titlename, x_showtitle, x_titlename, y_color, y_fontfamily, y_fontsize, y_fontstyle, y_align, x_color, x_fontfamily, x_fontsize, x_fontstyle, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, y_label_fontsize, x_label_fontsize, y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
}

//Using chartTypeName call function to push chart property variable value in array object
function getDivJsonData(chartTypeName) {
    if (chartTypeName == 'tabular') {
        json_dataValue_tableformat(cardColor, headingText, headingColor, headingFontName, headingFontSize, headingBold, headingItalic, headingUnderline, headingAlign, IconShow, TableHeaderColor, TableHeaderFontColor, TableHeaderFontName, TableHeaderFontSize, TableHeaderBold, TableHeaderItalic, TableHeaderUnderline, TableHeaderAlign, showHeader, showHeaderBorderShadow, TableCellFontColor, TableCellFontName, TableCellFontSize, TableCellBold, TableCellItalic, TableCellUnderline, TableCellAlign, TableCellBorderColor, OddCellColor, EvenCellColor, ShowPagination, showLayoutBorderShadow, showWrapText, showCompactNumber);
    }
    else if (chartTypeName == 'label') { }
    else {
        json_dataValue_chartformat(cardColor, headingText, headingColor, headingFontName, headingFontSize, headingBold, headingItalic, headingUnderline, headingAlign, IconShow, legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, x_showtitle, y_titlename, x_titlename, y_color, x_color, y_fontfamily, x_fontfamily, y_fontsize, x_fontsize, y_fontstyle, x_fontstyle, y_align, x_align, y_label_color, x_label_color, y_label_fontfamily, x_label_fontfamily, y_label_fontsize, x_label_fontsize, y_label_fontstyle, x_label_fontstyle, y_label_align, x_label_align);
    }

    var new_arr = getUniqueListBy(json_DataProperty, 'key');
    json_DataProperty = new_arr;
}
function getUniqueListBy(arr, key) {
    return [...new Map(arr.map(item => [item[key], item])).values()]
}
//Push chart property variable value in global array object(json_DataProperty)
function json_dataValue_chartformat(cardcolor, headtext, headcolor, headfontname, headfontsize, headbold, headitalic, headunderline, headalign, iconshow, font_color, font_size, font_family, label_align, legend_position, legend_style, show_legend, yAxis_fontsize, xAxis_fontsize, show_axis, show_gridlines, Y_showtitle, Y_titlename, X_showtitle, X_titlename, ycolor, yfontfamily, yfontsize, yfontstyle, yalign, xcolor, xfontfamily, xfontsize, xfontstyle, xalign, ylabelcolor, xlabelcolor, ylabelfontfamily, xlabelfontfamily, ylabelfontsize, xlabelfontsize, ylabelfontstyle, xlabelfontstyle, ylabelalign, xlabelalign) {
    var a = "cardColor,headingText,headingColor,headingFontName,headingFontSize,headingBold,headingItalic,headingUnderline,headingAlign,IconShow,legendColor,legendFontSize,legendFontName,legendAlign,legendPosition,legendStyle,legendShow,xAxisFontSize,yAxisFontSize,AxisShow,gridLinesShow,y_showtitle,x_showtitle,y_titlename,x_titlename,y_color,x_color,y_fontfamily,x_fontfamily,y_fontsize,x_fontsize,y_fontstyle,x_fontstyle,y_align,x_align,y_label_color,y_label_fontfamily,y_label_fontsize,y_label_fontstyle,y_label_align,x_label_color,x_label_fontfamily,x_label_fontsize,x_label_fontstyle,x_label_align";
    var label_val = a.split(",");
    var b = "" + cardcolor + "," + headtext + "," + headcolor + "," + headfontname + "," + headfontsize + "," + headbold + "," + headitalic + "," + headunderline + "," + headalign + "," + iconshow + "," + font_color + "," + font_size + "," + font_family + "," + label_align + "," + legend_position + "," + legend_style + "," + show_legend + "," + yAxis_fontsize + "," + xAxis_fontsize + "," + show_axis + "," + show_gridlines + "," + Y_showtitle + "," + Y_titlename + "," + X_showtitle + "," + X_titlename + "," + ycolor + "," + yfontfamily + "," + yfontsize + "," + yfontstyle + "," + yalign + "," + xcolor + "," + xfontfamily + "," + xfontsize + "," + xfontstyle + "," + xalign + "," + ylabelcolor + "," + ylabelfontfamily + "," + ylabelfontsize + "," + ylabelfontstyle + "," + ylabelalign + "," + xlabelcolor + "," + xlabelfontfamily + "," + xlabelfontsize + "," + xlabelfontstyle + "," + xlabelalign + "";
    var label_data = b.split(",");
    var chart_json_data = [];
    for (var i = 0; i < label_val.length; i++) {
        chart_json_data.push({
            key: label_val[i],
            value: label_data[i]
        });
        //chart_json_data.push({
        //    [label_val[i]]: label_data[i]
        //});
    }
    var sticky_name = 'sticky' + stickyID;
    json_DataProperty.push({
        key: sticky_name,
        value: chart_json_data
    });
    //console.log('CHART json_DataProperty:', json_DataProperty);
}
//Push table property variable value in global array object(json_DataProperty)
function json_dataValue_tableformat(cardcolor, headtext, headcolor, headfontname, headfontsize, headbold, headitalic, headunderline, headalign, iconshow, tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber) {
    var a = "cardColor,headingText,headingColor,headingFontName,headingFontSize,headingBold,headingItalic,headingUnderline,headingAlign,IconShow,TableHeaderColor,TableHeaderFontColor,TableHeaderFontName,TableHeaderFontSize,TableHeaderBold,TableHeaderItalic,TableHeaderUnderline,TableHeaderAlign,showHeader,showHeaderBorderShadow,TableCellFontColor,TableCellFontName,TableCellFontSize,TableCellBold,TableCellItalic,TableCellUnderline,TableCellAlign,TableCellBorderColor,OddCellColor,EvenCellColor,ShowPagination,showLayoutBorderShadow,showWrapText,showCompactNumber";
    var table_label_val = a.split(",");
    var b = "" + cardcolor + "," + headtext + "," + headcolor + "," + headfontname + "," + headfontsize + "," + headbold + "," + headitalic + "," + headunderline + "," + headalign + "," + iconshow + "," + tableheadercolor + "," + tableheaderfontcolor + "," + tableheaderfontname + "," + tableheaderfontsize + "," + tableheaderbold + "," + tableheaderitalic + "," + tableheaderunderline + "," + tableheaderalign + "," + showheader + "," + showheaderbordershadow + "," + tablecellfontcolor + "," + tablecellfontname + "," + tablecellfontsize + "," + tablecellbold + "," + tablecellitalic + "," + tablecellunderline + "," + tablecellalign + "," + tablecellbordercolor + "," + oddcellcolor + "," + evencellcolor + "," + showpagination + "," + showlayoutbordershadow + "," + showwraptext + "," + showcompactnumber + "";
    var table_label_data = b.split(",");
    var table_json_data = [];
    for (var i = 0; i < table_label_val.length; i++) {
        table_json_data.push({
            key: table_label_val[i],
            value: table_label_data[i]
        });
        //var obj = {};
        //obj[table_label_val[i]] = table_label_data[i];
        //table_json_data.push(obj);
    }
    var sticky_name = 'sticky' + stickyID;
    json_DataProperty.push({
        key: sticky_name,
        value: table_json_data
    });
    //console.log('TABLE json_DataProperty:', json_DataProperty);
}
//Get property variable value from JSON data
function getVarValue(data_json, key_element) {
    var result = data_json.filter(function (element) {
        return element.key == key_element;
    });

    if (result.length > 0) {
        var var_val = result[0].value;
        return var_val;
    }
}
//Re-Initialize the chart property variable
function chartValueInitialize() {
    cardColor = getVarValue(current_JSONDataProperty, 'cardColor');
    headingText = getVarValue(current_JSONDataProperty, 'headingText');
    headingColor = getVarValue(current_JSONDataProperty, 'headingColor');
    headingFontName = getVarValue(current_JSONDataProperty, 'headingFontName');
    headingFontSize = getVarValue(current_JSONDataProperty, 'headingFontSize');
    headingBold = getVarValue(current_JSONDataProperty, 'headingBold');
    headingItalic = getVarValue(current_JSONDataProperty, 'headingItalic');
    headingUnderline = getVarValue(current_JSONDataProperty, 'headingUnderline');
    headingAlign = getVarValue(current_JSONDataProperty, 'headingAlign');
    //IconShow = Boolean(getVarValue(current_JSONDataProperty, 'IconShow'));
    IconShow = getVarValue(current_JSONDataProperty, 'IconShow');
    IconShow = (IconShow == 'true');

    legendColor = getVarValue(current_JSONDataProperty, 'legendColor');
    legendFontSize = getVarValue(current_JSONDataProperty, 'legendFontSize');
    legendFontName = getVarValue(current_JSONDataProperty, 'legendFontName');
    legendAlign = getVarValue(current_JSONDataProperty, 'legendAlign');
    legendPosition = getVarValue(current_JSONDataProperty, 'legendPosition');
    legendStyle = getVarValue(current_JSONDataProperty, 'legendStyle');
    //legendShow = Boolean(getVarValue(current_JSONDataProperty, 'legendShow'));
    legendShow = getVarValue(current_JSONDataProperty, 'legendShow');
    legendShow = (legendShow == 'true');
    xAxisFontSize = getVarValue(current_JSONDataProperty, 'xAxisFontSize');
    yAxisFontSize = getVarValue(current_JSONDataProperty, 'yAxisFontSize');
    //AxisShow = Boolean(getVarValue(current_JSONDataProperty, 'AxisShow'));
    //gridLinesShow = Boolean(getVarValue(current_JSONDataProperty, 'gridLinesShow'));
    AxisShow = getVarValue(current_JSONDataProperty, 'AxisShow');
    AxisShow = (AxisShow == 'true');
    gridLinesShow = getVarValue(current_JSONDataProperty, 'gridLinesShow');
    gridLinesShow = (gridLinesShow == 'true');
    //y_showtitle = Boolean(getVarValue(current_JSONDataProperty, 'y_showtitle'));
    y_showtitle = getVarValue(current_JSONDataProperty, 'y_showtitle');
    y_showtitle = (y_showtitle == 'true');
    //x_showtitle = Boolean(getVarValue(current_JSONDataProperty, 'x_showtitle'));
    x_showtitle = getVarValue(current_JSONDataProperty, 'x_showtitle');
    x_showtitle = (x_showtitle == 'true');
    //IconShow = getVarValue(current_JSONDataProperty, 'IconShow');
    //IconShow = (IconShow == 'true');
    y_titlename = getVarValue(current_JSONDataProperty, 'y_titlename');
    x_titlename = getVarValue(current_JSONDataProperty, 'x_titlename');
    y_color = getVarValue(current_JSONDataProperty, 'y_color');
    x_color = getVarValue(current_JSONDataProperty, 'x_color');
    y_fontfamily = getVarValue(current_JSONDataProperty, 'y_fontfamily');
    x_fontfamily = getVarValue(current_JSONDataProperty, 'x_fontfamily');
    y_fontsize = getVarValue(current_JSONDataProperty, 'y_fontsize');
    x_fontsize = getVarValue(current_JSONDataProperty, 'x_fontsize');
    y_fontstyle = getVarValue(current_JSONDataProperty, 'y_fontstyle');
    x_fontstyle = getVarValue(current_JSONDataProperty, 'x_fontstyle');
    y_align = getVarValue(current_JSONDataProperty, 'y_align');
    x_align = getVarValue(current_JSONDataProperty, 'x_align');
    y_label_color = getVarValue(current_JSONDataProperty, 'y_label_color');
    x_label_color = getVarValue(current_JSONDataProperty, 'x_label_color');
    y_label_fontfamily = getVarValue(current_JSONDataProperty, 'y_label_fontfamily');
    x_label_fontfamily = getVarValue(current_JSONDataProperty, 'x_label_fontfamily');
    y_label_fontsize = getVarValue(current_JSONDataProperty, 'y_label_fontsize');
    x_label_fontsize = getVarValue(current_JSONDataProperty, 'x_label_fontsize');
    y_label_fontstyle = getVarValue(current_JSONDataProperty, 'y_label_fontstyle');
    x_label_fontstyle = getVarValue(current_JSONDataProperty, 'x_label_fontstyle');
    y_label_align = getVarValue(current_JSONDataProperty, 'y_label_align');
    x_label_align = getVarValue(current_JSONDataProperty, 'x_label_align');
}
//Re-Initialize the table property variable
function tableValueInitialize() {
    cardColor = getVarValue(current_JSONDataProperty, 'cardColor');
    headingText = getVarValue(current_JSONDataProperty, 'headingText');
    headingColor = getVarValue(current_JSONDataProperty, 'headingColor');
    headingFontName = getVarValue(current_JSONDataProperty, 'headingFontName');
    headingFontSize = getVarValue(current_JSONDataProperty, 'headingFontSize');
    headingBold = getVarValue(current_JSONDataProperty, 'headingBold');
    headingItalic = getVarValue(current_JSONDataProperty, 'headingItalic');
    headingUnderline = getVarValue(current_JSONDataProperty, 'headingUnderline');
    headingAlign = getVarValue(current_JSONDataProperty, 'headingAlign');
    //IconShow = Boolean(getVarValue(current_JSONDataProperty, 'IconShow'));
    IconShow = getVarValue(current_JSONDataProperty, 'IconShow');
    IconShow = (IconShow == 'true');

    TableHeaderColor = getVarValue(current_JSONDataProperty, 'TableHeaderColor');
    TableHeaderFontColor = getVarValue(current_JSONDataProperty, 'TableHeaderFontColor');
    TableHeaderFontName = getVarValue(current_JSONDataProperty, 'TableHeaderFontName');
    TableHeaderFontSize = getVarValue(current_JSONDataProperty, 'TableHeaderFontSize');
    TableHeaderBold = getVarValue(current_JSONDataProperty, 'TableHeaderBold');
    TableHeaderItalic = getVarValue(current_JSONDataProperty, 'TableHeaderItalic');
    TableHeaderUnderline = getVarValue(current_JSONDataProperty, 'TableHeaderUnderline');
    TableHeaderAlign = getVarValue(current_JSONDataProperty, 'TableHeaderAlign');
    //showHeader = Boolean(getVarValue(current_JSONDataProperty, 'showHeader'));
    //showHeaderBorderShadow = Boolean(getVarValue(current_JSONDataProperty, 'showHeaderBorderShadow'));
    showHeader = getVarValue(current_JSONDataProperty, 'showHeader');
    showHeader = (showHeader == 'true');
    showHeaderBorderShadow = getVarValue(current_JSONDataProperty, 'showHeaderBorderShadow');
    showHeaderBorderShadow = (showHeaderBorderShadow == 'true');
    TableCellFontColor = getVarValue(current_JSONDataProperty, 'TableCellFontColor');
    TableCellFontName = getVarValue(current_JSONDataProperty, 'TableCellFontName');
    TableCellFontSize = getVarValue(current_JSONDataProperty, 'TableCellFontSize');
    TableCellBold = getVarValue(current_JSONDataProperty, 'TableCellBold');
    TableCellItalic = getVarValue(current_JSONDataProperty, 'TableCellItalic');
    TableCellUnderline = getVarValue(current_JSONDataProperty, 'TableCellUnderline');
    TableCellAlign = getVarValue(current_JSONDataProperty, 'TableCellAlign');
    TableCellBorderColor = getVarValue(current_JSONDataProperty, 'TableCellBorderColor');
    OddCellColor = getVarValue(current_JSONDataProperty, 'OddCellColor');
    EvenCellColor = getVarValue(current_JSONDataProperty, 'EvenCellColor');
    //ShowPagination = Boolean(getVarValue(current_JSONDataProperty, 'ShowPagination'));
    //showLayoutBorderShadow = Boolean(getVarValue(current_JSONDataProperty, 'showLayoutBorderShadow'));
    //showWrapText = Boolean(getVarValue(current_JSONDataProperty, 'showWrapText'));
    //showCompactNumber = Boolean(getVarValue(current_JSONDataProperty, 'showCompactNumber'));
    ShowPagination = getVarValue(current_JSONDataProperty, 'ShowPagination');
    ShowPagination = (ShowPagination == 'true');
    showLayoutBorderShadow = getVarValue(current_JSONDataProperty, 'showLayoutBorderShadow');
    showLayoutBorderShadow = (showLayoutBorderShadow == 'true');
    showWrapText = getVarValue(current_JSONDataProperty, 'showWrapText');
    showWrapText = (showWrapText == 'true');
    showCompactNumber = getVarValue(current_JSONDataProperty, 'showCompactNumber');
    showCompactNumber = (showCompactNumber == 'true');
}
//Re-Initialize the property variable
function reinitializeVariable() {
    cardColor = '#fff';
    headingText = StickyTablename;
    headingColor = '#000';
    headingFontName = FontName;
    headingFontSize = FontSize;
    headingBold = 'normal';
    headingItalic = 'normal';
    headingUnderline = 'normal';
    headingAlign = 'left';
    IconShow = Boolean(false);
    TableHeaderColor = '';
    //TableHeaderColor = '#fff';
    TableHeaderFontColor = '#000';
    TableHeaderFontName = FontName;
    TableHeaderFontSize = '10';
    TableHeaderBold = 'normal';
    TableHeaderItalic = 'normal';
    TableHeaderUnderline = 'normal';
    TableHeaderAlign = 'left';
    showHeader = Boolean(true);
    showHeaderBorderShadow = Boolean(false);
    TableCellFontColor = '';
    TableCellFontName = FontName;
    TableCellFontSize = '10';
    TableCellBold = 'normal';
    TableCellItalic = 'normal';
    TableCellUnderline = 'normal';
    TableCellAlign = 'left';
    TableCellBorderColor = '#ccc';
    OddCellColor = '';
    EvenCellColor = '';
    ShowPagination = Boolean(true);
    showLayoutBorderShadow = Boolean(false);
    showWrapText = Boolean(false);
    showCompactNumber = Boolean(false);
    legendColor = '#000';
    legendFontSize = '10';
    legendFontName = FontName;
    legendAlign = 'center';
    legendPosition = 'left';
    legendStyle = 'normal';
    legendShow = Boolean(false);
    xAxisFontSize = '10';
    yAxisFontSize = '10';
    AxisShow = Boolean(true);
    gridLinesShow = Boolean(true);
    y_showtitle = Boolean(false);
    x_showtitle = Boolean(false);
    y_titlename = '';
    x_titlename = '';
    y_color = '#000';
    x_color = '#000';
    y_fontfamily = FontName;
    x_fontfamily = FontName;
    y_fontsize = '10';
    x_fontsize = '10';
    y_fontstyle = 'normal';
    x_fontstyle = 'normal';
    y_align = 'center';
    x_align = 'center';
    y_label_color = '#000';
    x_label_color = '#000';
    y_label_fontfamily = FontName;
    x_label_fontfamily = FontName;
    y_label_fontsize = '10';
    x_label_fontsize = '10';
    y_label_fontstyle = 'normal';
    x_label_fontstyle = 'normal';
    y_label_align = 'center';
    x_label_align = 'center';
}

function checkChartValue(font_color, font_size, font_family, label_align, legend_position, legend_style, show_legend, yAxis_fontsize, xAxis_fontsize, show_axis, show_gridlines, Y_showtitle, Y_titlename, X_showtitle, X_titlename, ycolor, yfontfamily, yfontsize, yfontstyle, yalign, xcolor, xfontfamily, xfontsize, xfontstyle, xalign, ylabelcolor, xlabelcolor, ylabelfontfamily, xlabelfontfamily, ylabelfontsize, xlabelfontsize, ylabelfontstyle, xlabelfontstyle, ylabelalign, xlabelalign) {
    if (font_color != '') {
        legendColor = font_color;
    }
    if (font_size != '') {
        legendFontSize = font_size;
    }
    if (font_family != '') {
        legendFontName = font_family;
    }
    if (label_align != '') {
        legendAlign = label_align;
    }
    if (legend_position != '') {
        legendPosition = legend_position;
    }
    if (legend_style != '') {
        legendStyle = legend_style;
    }
    if (yAxis_fontsize != '') {
        yAxisFontSize = yAxis_fontsize;
    }
    if (xAxis_fontsize != '') {
        xAxisFontSize = xAxis_fontsize;
    }
    if (Y_titlename != '') {
        y_titlename = Y_titlename;
    }
    if (X_titlename != '') {
        x_titlename = X_titlename;
    }
    if (ycolor != '') {
        y_color = ycolor;
    }
    if (yfontfamily != '') {
        y_fontfamily = yfontfamily;
    }
    if (yfontsize != '') {
        y_fontsize = yfontsize;
    }
    if (yalign != '') {
        y_align = yalign;
    }
    if (yfontstyle != '') {
        y_fontstyle = yfontstyle;
    }
    if (xcolor != '') {
        x_color = xcolor;
    }
    if (xfontfamily != '') {
        x_fontfamily = xfontfamily;
    }
    if (xfontsize != '') {
        x_fontsize = xfontsize;
    }
    if (xalign != '') {
        x_align = xalign;
    }
    if (xfontstyle != '') {
        x_fontstyle = xfontstyle;
    }
    if (ylabelcolor != '') {
        y_label_color = ylabelcolor;
    }
    if (ylabelfontfamily != '') {
        y_label_fontfamily = ylabelfontfamily;
    }
    if (ylabelfontsize != '') {
        y_label_fontsize = ylabelfontsize;
    }
    if (ylabelalign != '') {
        y_label_align = ylabelalign;
    }
    if (ylabelfontstyle != '') {
        y_label_fontstyle = ylabelfontstyle;
    }
    if (xlabelcolor != '') {
        x_label_color = xlabelcolor;
    }
    if (xlabelfontfamily != '') {
        x_label_fontfamily = xlabelfontfamily;
    }
    if (xlabelfontsize != '') {
        x_label_fontsize = xlabelfontsize;
    }
    if (xlabelalign != '') {
        x_label_align = xlabelalign;
    }
    if (xlabelfontstyle != '') {
        x_label_fontstyle = xlabelfontstyle;
    }
    legendShow = (show_legend == 'true');
    AxisShow = (show_axis == 'true');
    gridLinesShow = (show_gridlines == 'true');
    y_showtitle = (Y_showtitle == 'true');
    x_showtitle = (X_showtitle == 'true');
}
function checkCardValue(card_color, heading_text, heading_color, heading_fontName, heading_fontSize, heading_bold, heading_italic, heading_underline, heading_align, icon_show) {
    if (card_color != '') {
        cardColor = card_color;
    }
    if (heading_text != '') {
        headingText = heading_text;
    }
    if (heading_color != '') {
        headingColor = heading_color;
    }
    if (heading_fontName != '') {
        headingFontName = heading_fontName;
    }
    if (heading_fontSize != '') {
        headingFontSize = heading_fontSize;
    }
    if (heading_bold != '') {
        headingBold = heading_bold;
    }
    if (heading_italic != '') {
        headingItalic = heading_italic;
    }
    if (heading_underline != '') {
        headingUnderline = heading_underline;
    }
    if (heading_align != '') {
        headingAlign = heading_align;
    }
    IconShow = (icon_show == 'true');
}
function checkTabularValue(tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber) {
    if (tableheadercolor != '') {
        TableHeaderColor = tableheadercolor;
    }
    if (tableheaderfontcolor != '') {
        TableHeaderFontColor = tableheaderfontcolor;
    }
    if (tableheaderfontname != '') {
        TableHeaderFontName = tableheaderfontname;
    }
    if (tableheaderfontsize != '') {
        TableHeaderFontSize = tableheaderfontsize;
    }
    if (tableheaderbold != '') {
        TableHeaderBold = tableheaderbold;
    }
    if (tableheaderitalic != '') {
        TableHeaderItalic = tableheaderitalic;
    }
    if (tableheaderunderline != '') {
        TableHeaderUnderline = tableheaderunderline;
    }
    if (tableheaderalign != '') {
        TableHeaderAlign = tableheaderalign;
    }
    if (tablecellfontcolor != '') {
        TableCellFontColor = tablecellfontcolor;
    }
    if (tablecellfontname != '') {
        TableCellFontName = tablecellfontname;
    }
    if (tablecellfontsize != '') {
        TableCellFontSize = tablecellfontsize;
    }
    if (tablecellbold != '') {
        TableCellBold = tablecellbold;
    }
    if (tablecellitalic != '') {
        TableCellItalic = tablecellitalic;
    }
    if (tablecellunderline != '') {
        TableCellUnderline = tablecellunderline;
    }
    if (tablecellalign != '') {
        TableCellAlign = tablecellalign;
    }
    if (tablecellbordercolor != '') {
        TableCellBorderColor = tablecellbordercolor;
    }
    if (oddcellcolor != '') {
        OddCellColor = oddcellcolor;
    }
    if (evencellcolor != '') {
        EvenCellColor = evencellcolor;
    }
    showHeader = (showheader == 'true');
    showHeaderBorderShadow = (showheaderbordershadow == 'true');
    ShowPagination = (showpagination == 'true');
    showLayoutBorderShadow = (showlayoutbordershadow == 'true');
    showWrapText = (showwraptext == 'true');
    showCompactNumber = (showcompactnumber == 'true');
}

//Edit Dashbaord Data 
function getReadyData(kpi_items) {
    //var D_id = 1;
    //var chartId = 1;
    var kpiList = kpi_items;
    var itemCount = kpiList.length;
    var divId = itemCount + 1;
    //var chart_id = itemCount + 1;
    D_id = itemCount + 1;

    if (FontName == '' && FontName == undefined) {
        FontName = 'arial';
    }
    if (FontSize == '' && FontSize == undefined) {
        FontSize = '10';
    }
    myGrid = new MyGrid('#grid', 'tile_');

    for (var i = 0; i < kpiList.length; i++) {
        var jsonData = kpiList[i].json_tuple;
        var chartName = kpiList[i].kpi_name;
        var dataval = kpiList[i].chart_params;
        var chartType = kpiList[i].chart_type;
        var chart_caption = kpiList[i].chart_caption;
        var chart_color = kpiList[i].chart_color;
        var QueryId = kpiList[i].kpi_query_id;
        var KpiId = kpiList[i].kpi_id;
        var chartproperty = kpiList[i].json_property;

        var chart_position = kpiList[i].chart_position;
        var chart_pos = eval('(' + chart_position + ')');
        var width_chart = chart_pos["chart_width"];
        var height_chart = chart_pos["chart_height"];
        var top_chart = chart_pos["chart_top"];
        var left_chart = chart_pos["chart_left"];

        var kpi_x = chart_pos["kpi_x"];
        var kpi_y = chart_pos["kpi_y"];
        var kpi_width = chart_pos["kpi_width"];
        var kpi_height = chart_pos["kpi_height"];

        console.log("kpi_x:", kpi_x);
        console.log("kpi_y:", kpi_y);
        console.log("kpi_width:", kpi_width);
        console.log("kpi_height:", kpi_height);

        var tval = eval('(' + dataval + ')');
        var editNote = '';
        headingText = chartName;
        stickyID = chart_id;
        chartTypeName = chartType;
        var kpi_pro = '';
        var kpi_property = kpiList[i].kpi_property;

        if (kpi_property != null && kpi_property != '') {
            kpi_pro = JSON.parse(kpi_property);
            kpiProperty = kpi_pro;
            //console.log("kpi_pro:", kpi_pro);
            checkCardValue(kpi_pro["cardColor"], kpi_pro["headingText"], kpi_pro["headingColor"], kpi_pro["headingFontName"], kpi_pro["headingFontSize"], kpi_pro["headingBold"], kpi_pro["headingItalic"], kpi_pro["headingUnderline"], kpi_pro["headingAlign"], kpi_pro["IconShow"]);

            if (chartType.toLowerCase() != 'tabular') {
                checkChartValue(kpi_pro["legendColor"], kpi_pro["legendFontSize"], kpi_pro["legendFontName"], kpi_pro["legendAlign"], kpi_pro["legendPosition"], kpi_pro["legendStyle"], kpi_pro["legendShow"], kpi_pro["xAxisFontSize"], kpi_pro["yAxisFontSize"], kpi_pro["AxisShow"], kpi_pro["gridLinesShow"], kpi_pro["y_showtitle"], kpi_pro["y_titlename"], kpi_pro["x_showtitle"], kpi_pro["x_titlename"], kpi_pro["y_color"], kpi_pro["y_fontfamily"], kpi_pro["y_fontsize"], kpi_pro["y_fontstyle"], kpi_pro["y_align"], kpi_pro["x_color"], kpi_pro["x_fontfamily"], kpi_pro["x_fontsize"], kpi_pro["x_fontstyle"], kpi_pro["x_align"], kpi_pro["y_label_color"], kpi_pro["x_label_color"], kpi_pro["y_label_fontfamily"], kpi_pro["x_label_fontfamily"], kpi_pro["y_label_fontsize"], kpi_pro["x_label_fontsize"], kpi_pro["y_label_fontstyle"], kpi_pro["x_label_fontstyle"], kpi_pro["y_label_align"], kpi_pro["x_label_align"]);
            }
            else {
                checkTabularValue(kpi_pro["TableHeaderColor"], kpi_pro["TableHeaderFontColor"], kpi_pro["TableHeaderFontName"], kpi_pro["TableHeaderFontSize"], kpi_pro["TableHeaderBold"], kpi_pro["TableHeaderItalic"], kpi_pro["TableHeaderUnderline"], kpi_pro["TableHeaderAlign"], kpi_pro["ShowHeader"], kpi_pro["ShowHeaderBorderShadow"], kpi_pro["TableCellFontColor"], kpi_pro["TableCellFontName"], kpi_pro["TableCellFontSize"], kpi_pro["TableCellBold"], kpi_pro["TableCellItalic"], kpi_pro["TableCellUnderline"], kpi_pro["TableCellAlign"], kpi_pro["TableCellBorderColor"], kpi_pro["OddCellColor"], kpi_pro["EvenCellColor"], kpi_pro["ShowPagination"], kpi_pro["showLayoutBorderShadow"], kpi_pro["showWrapText"], kpi_pro["showCompactNumber"]);
            }

            getDivJsonData(chartTypeName);
            //property_values(stickyID);
            edit_property_values(stickyID, chartTypeName)

        }
        else {
            reinitializeVariable();
        }

        if (chartType.toLowerCase() == 'tabular') {
            //getDivJsonData(chartTypeName);
            //property_values(stickyID);
            //json_dataValue_tableformat(cardColor, headingText, headingColor, headingFontName, headingFontSize, headingBold, headingItalic, headingUnderline, headingAlign, IconShow, TableHeaderColor, TableHeaderFontColor, TableHeaderFontName, TableHeaderFontSize, TableHeaderBold, TableHeaderItalic, TableHeaderUnderline, TableHeaderAlign, showHeader, showHeaderBorderShadow, TableCellFontColor, TableCellFontName, TableCellFontSize, TableCellBold, TableCellItalic, TableCellUnderline, TableCellAlign, TableCellBorderColor, OddCellColor, EvenCellColor, ShowPagination, showLayoutBorderShadow, showWrapText, showCompactNumber);

            editNote = $("<div><div class='grid-stack-item-content tile card plainCard' id='sticky" + chart_id + "' data-id='" + KpiId + "' data-val-number='" + chartType + "' data-val='" + QueryId + "' data-content='" + chartproperty + "'>"
                + "<div class='card-header border-0'>"
                + "<div class='d-flex'>"
                + "<h3 class='mr-2 card-title w-100' id='mainHeader" + chart_id + "'>" + headingText + "</h3>"
                + "<button id='" + chart_id + "' class='btn bg-info btn-sm openbtn' type='button' onclick='openNav(" + chart_id + ")' style='margin-right:5px;'>"
                + "<i class='fas fa-cog'></i></button>"
                + "<button type='button' class='btn bg-info btn-sm removebtn' id='" + chart_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
                + "<i class='fa fa-times'></i>"
                + "</button></div></div>"
                + "<div class='card-body chartCard" + chart_id + "' style='overflow:auto'>"
                + "<table id='myChart" + chart_id + "' class='chartCanvas'></table>"
                + "</div></div></div>");

            //myGrid = new MyGrid('#grid', 'tile_');
            //myGrid.addTile(editNote);
            //myGrid.addTile(editNote, kpi_x, kpi_y, kpi_width, kpi_height);
            myGrid.addTile(editNote, 'edit', kpi_x, kpi_y, kpi_width, kpi_height);
            if (headingText == '' || headingText == null) {
                headingText = chartName;
            }
            $('#mainHeader' + chart_id).html(headingText);
            if (IconShow == true) {
                var titlename = $('#mainHeader' + chart_id).text();
                $('#mainHeader' + chart_id).text('');
                var btn = document.createElement("i");
                btn.setAttribute("class", "fa fa-th mr-1");
                $('#mainHeader' + chart_id).append(btn);
                $('#mainHeader' + chart_id).append(titlename);
                $('#mainHeader' + chart_id).find('i').css("display", "");
            }
            //This function is in EditDashboardDemo.cshtml file
            DrawTheTable(tval, dataval, QueryId, chartType, chart_id, kpi_pro["TableHeaderColor"], kpi_pro["TableHeaderFontColor"], kpi_pro["TableHeaderFontName"], kpi_pro["TableHeaderFontSize"], kpi_pro["TableHeaderBold"], kpi_pro["TableHeaderItalic"], kpi_pro["TableHeaderUnderline"], kpi_pro["TableHeaderAlign"], kpi_pro["showHeader"], kpi_pro["showHeaderBorderShadow"], kpi_pro["TableCellFontColor"], kpi_pro["TableCellFontName"], kpi_pro["TableCellFontSize"], kpi_pro["TableCellBold"], kpi_pro["TableCellItalic"], kpi_pro["TableCellUnderline"], kpi_pro["TableCellAlign"], kpi_pro["TableCellBorderColor"], kpi_pro["OddCellColor"], kpi_pro["EvenCellColor"], kpi_pro["ShowPagination"], kpi_pro["showLayoutBorderShadow"], kpi_pro["showWrapText"], kpi_pro["showCompactNumber"]);
        }
        else if (chartType.toLowerCase() == 'label') {
            if (headingText == '' || headingText == null) {
                headingText = chartName;
            }
            editNote = $("<div><div class='grid-stack-item-content tile card plainCard' id='sticky" + chart_id + "' data-id='" + KpiId + "' data-val-number='" + chartType + "' data-val='" + QueryId + "' data-content='" + chartproperty + "'>"
                + "<div class= 'card-header border-0'><h3 class='card-title'>"
                + "<i class='fa fa-th mr-1'></i>" + headingText + "</h3>"
                + "<div class='card-tools'>"
                + "<button type='button' class='btn bg-info btn-sm removebtn' id='" + chart_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
                + "<i class='fa fa-times'></i>"
                + "</button></div></div>"
                + "<div class='card-body small-boxx chartCard" + chart_id + "' id='myChart" + chart_id + "'>"
                + "</div></div></div>");

            //myGrid = new MyGrid('#grid', 'tile_');
            //myGrid.addTile(editNote);
            myGrid.addTile(editNote, 'edit', kpi_x, kpi_y, kpi_width, kpi_height);
            //myGrid.addTile(editNote, kpi_x, kpi_y, kpi_width, kpi_height);
            //This function is in EditDashboardDemo.cshtml file
            DrawTheLabel(tval, dataval, QueryId, chartType, chart_id, chart_caption, chart_color);
        }
        else {
             //getDivJsonData(chartTypeName);
             //property_values(stickyID);
             //json_dataValue_chartformat(cardColor, headingText, headingColor, headingFontName, headingFontSize, headingBold, headingItalic, headingUnderline, headingAlign, IconShow, legendColor, legendFontSize, legendFontName, legendAlign, legendPosition, legendStyle, legendShow, xAxisFontSize, yAxisFontSize, AxisShow, gridLinesShow, y_showtitle, x_showtitle, y_titlename, x_titlename, y_color, x_color, y_fontfamily, x_fontfamily, y_fontsize, x_fontsize, y_fontstyle, x_fontstyle, y_align, x_align, y_label_color, y_label_fontfamily, y_label_fontsize, y_label_fontstyle, y_label_align, x_label_color, x_label_fontfamily, x_label_fontsize, x_label_fontstyle, x_label_align);

             editNote = $("<div><div class='grid-stack-item-content tile card plainCard' id='sticky" + chart_id + "' data-id='" + KpiId + "' data-val-number='" + chartType + "' data-val='" + QueryId + "' data-content='" + chartproperty + "'>"
                + "<div class='card-header border-0'>"
                + "<div class='d-flex'>"
                + "<h3 class='mr-2 card-title w-100' id='mainHeader" + chart_id + "'>" + headingText + "</h3>"
                + "<button id='" + chart_id + "' class='btn bg-info btn-sm openbtn' type='button' onclick='openNav(" + chart_id + ")' style='margin-right:5px;'>"
                + "<i class='fas fa-cog'></i></button>"
                + "<button type='button' class='btn bg-info btn-sm daterange' data-toggle='tooltip' title='Date range' style='margin-right:5px;'>"
                + "<i class='fa fa-calendar'></i></button>"
                + "<button type='button' class='btn bg-info btn-sm removebtn' id='" + chart_id + "' onclick='delTile(this)' data-card-widget='remove' data-toggle='tooltip' title='Remove'>"
                + "<i class='fa fa-times'></i>"
                + "</button></div></div>"
                + "<div class='card-body chartCard" + chart_id + "'>"
                + "<canvas id='myChart" + chart_id + "' class='chartCanvas'></canvas>"
                + "</div></div></div>");

            //myGrid = new MyGrid('#grid', 'tile_');
            //myGrid.addTile(editNote);
            myGrid.addTile(editNote, 'edit', kpi_x, kpi_y, kpi_width, kpi_height);
            //myGrid.addTile(editNote, kpi_x, kpi_y, kpi_width, kpi_height);
            if (headingText == '' || headingText == null) {
                headingText = chartName;
            }
            $('#mainHeader' + chart_id).html(headingText);
            if (IconShow == true) {
                var titlename = $('#mainHeader' + chart_id).text();
                $('#mainHeader' + chart_id).text('');
                var btn = document.createElement("i");
                btn.setAttribute("class", "fa fa-th mr-1");
                $('#mainHeader' + chart_id).append(btn);
                $('#mainHeader' + chart_id).append(titlename);
                $('#mainHeader' + chart_id).find('i').css("display", "");
            }
            DrawTheGraphData([tval], jsonData, chartType, chart_id);
        }

        //$('#sticky' + chartId).css("width", width_chart);
        //$('#sticky' + chartId).css("height", height_chart);
        //$('#sticky' + chartId).css("top", top_chart);
        //$('#sticky' + chartId).css("left", left_chart);
        //$('#main-container').css("position", "absolute");

        $('#sticky' + chart_id).css("background", cardColor);
        //$('#sticky' + chart_id).css("position", "relative");
        $('#mainHeader' + chart_id).css("color", headingColor);
        $('#mainHeader' + chart_id).css("text-align", headingAlign);
        $('#mainHeader' + chart_id).css("font-family", headingFontName);
        $('#mainHeader' + chart_id).css("font-size", parseInt(headingFontSize));
        $('#mainHeader' + chart_id).css("font-weight", headingBold);
        $('#mainHeader' + chart_id).css("font-style", headingItalic);
        $('#mainHeader' + chart_id).css("text-decoration", headingUnderline);

        chart_id++;
    }

}




