var FontName = '';
var FontSize = '';
function getDashboardShowData(fontname, fontsize) {
    FontName = fontname;
    FontSize = fontsize;
}

var myChart;
function DrawTheGraph(data, name, jsonData, chartId) {
    var t = [data];
    var xvl;
    var hvl;
    var gvl;
    var cvlOld;
    var cvl = [];
    var dvl;
    var highvl;
    var lowvl;
    var openvl;
    var closevl;
    var thvl;
    var actualHeightData = [];
    var thresholdVal = [];
    var myObject = eval('(' + jsonData + ')');
    let obj = Object.keys(t[0]);
    let ArrayData = [];
    for (let i = 0; i < obj.length; i++) {
        let ArrayItem = obj[i];
        if (ArrayItem == 'Xaxis') {
            xvl = t[0][ArrayItem];
            let data = {
                key: ArrayItem,
                value: myObject.map((item) => {
                    return item[xvl];
                })
            }
            ArrayData.push(data);
        }
        else if (ArrayItem == 'Height') {
            hvl = t[0][ArrayItem].split(",");
            for (let j = 0; j < hvl.length; j++) {
                var dataval = hvl[j];
                let data = {
                    //key: dataval,
                    key: ArrayItem,
                    value: myObject.map((item) => {
                        return item[dataval];
                    })
                }
                let data1 = {
                    key: dataval,
                    value: myObject.map((item) => {
                        return item[dataval];
                    })
                }
                actualHeightData.push(data1);
                ArrayData.push(data);
            }
        }
        else if (ArrayItem == 'Threshold') {
            thvl = t[0][ArrayItem].split(",");
            for (let j = 0; j < thvl.length; j++) {
                var dataval = thvl[j];
                let data = {
                    key: dataval,
                    value: myObject.map((item) => {
                        return item[dataval];
                    })
                }
                thresholdVal.push(data);
                ArrayData.push(data);
            }
        }
        else if (ArrayItem == 'Group') {
            gvl = t[0][ArrayItem];
            let data = {
                key: ArrayItem,
                value: myObject.map((item) => {
                    return item[gvl];
                })
            }
            ArrayData.push(data);
        }
        else if (ArrayItem.toLowerCase() == 'color') {
            //cvlOld = t[0][ArrayItem].split("),");
            cvlOld = t[0][ArrayItem].split(/,(?![^()]*\))\s*/g);
            for (let j = 0; j < cvlOld.length; j++) {
                var color_dataval = cvlOld[j];
                //color_dataval = color_dataval + ")";
                //color_dataval = color_dataval.replace("))", ")");
                cvl.push(color_dataval);
            }
        }
        else if (ArrayItem == 'Date') {
            dvl = t[0][ArrayItem];
            let data = {
                key: ArrayItem,
                value: myObject.map((item) => {
                    return item[dvl];
                })
            }
            ArrayData.push(data);
        }
        else if (ArrayItem == 'High') {
            openvl = t[0][ArrayItem];
            let data = {
                key: ArrayItem,
                value: myObject.map((item) => {
                    return item[openvl];
                })
            }
            ArrayData.push(data);
        }
        else if (ArrayItem == 'Low') {
            lowvl = t[0][ArrayItem];
            let data = {
                key: ArrayItem,
                value: myObject.map((item) => {
                    return item[lowvl];
                })
            }
            ArrayData.push(data);
        }
        else if (ArrayItem == 'Open') {
            openvl = t[0][ArrayItem];
            let data = {
                key: ArrayItem,
                value: myObject.map((item) => {
                    return item[openvl];
                })
            }
            ArrayData.push(data);
        }
        else if (ArrayItem == 'Close') {
            closevl = t[0][ArrayItem];
            let data = {
                key: ArrayItem,
                value: myObject.map((item) => {
                    return item[closevl];
                })
            }
            ArrayData.push(data);
        }

        else { }
    }

    DrawDynamicGraph(ArrayData, actualHeightData, thresholdVal, name, xvl, gvl, hvl, cvl, dvl, highvl, lowvl, openvl, closevl, chartId);
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
    if (cvll == null || cvll.length == 0) {
        cvll = ["lightblue", "lightgreen", "pink", "#FFD580", "#cb504d", "yellow", "purple", "gray", "violet", "black"];
    }
    var datasset = [];
    var heightValueData = Hightvalue[0];

    var colors = ["lightblue", "lightgreen", "pink", "#FFD580", "#cb504d", "yellow", "purple", "gray", "violet", "black"];//#FFD580(orange), #cb504d(light red)
    for (let i = 0; i < actualHeightData.length; i++) {
        datasset.push({
            label: hvll[i],
            data: actualHeightData[i].value,
            backgroundColor: cvll[i],
            borderColor: cvll[i]
        });
    }

    if (newType.toLowerCase() == 'pie' || newType.toLowerCase() == 'doughnut' || newType.toLowerCase() == 'polararea') {
        var config = {
            type: newType,
            data: {
                labels: Xaxisvalue,
                datasets: [
                    {
                        backgroundColor: ["Green", "Blue", "orange", "Purple", "Yellow", "Red", "Black", "purple", "violet", "Gray"],
                        //backgroundColor: colors,
                        //backgroundColor: ["Green", "Blue", "Gray", "pink", "lightblue", "lightgreen", "#FFD580", "#cb504d", "yellow", "purple", "gray", "violet", "black"],
                        //backgroundColor: ["#FFD580", "#cb504d", "lightblue", "lightgreen", "pink", "yellow", "orange", "purple", "violet", "blue", "green", "red"],
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
                //colors = ["red", "yellow", "green", "lightblue", "lightgreen", "pink", "#FFD580", "#cb504d", "purple", "gray", "violet", "black"];

                newType = "line";
                splin = 0.6;
                fl = false;
                sl = false;
                stked = false;
            }

            for (let i = 0; i < actualHeightData.length; i++) {
                datasset.push({
                    label: hvll[i],
                    //backgroundColor: colors[i],
                    //borderColor: colors[i],
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
                    backgroundColor: colors[i],
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
                    tooltips: {
                        enabled: true
                    }
                },
                elements: {
                    line: {
                        borderWidth: 3
                    }
                }
            },
        };
    }
    else if (newType.toLowerCase() == "stacked_bar") {
        newType = 'bar';

        //datasset = [];
        //for (let i = 0; i < actualHeightData.length; i++) {
        //    datasset.push({
        //        label: actualHeightData[i].key,
        //        data: actualHeightData[i].value,
        //        backgroundColor: cvll[i],
        //        borderColor: cvll[i],
        //        hoverBackgroundColor: cvll[i],
        //        hoverBorderColor: cvll[i]
        //    });
        //}

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
            type: 'line',
            data: {
                labels: Xaxisvalue,
                datasets: datasset,
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    intersect: false,
                    axis: 'x'
                },
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

        //console.log("ARRAY:", arr);

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
                    hoverBackgroundColor: colors[1]
                    //backgroundColor: 'rgba(75, 00, 150, 0.2)'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: legendShow
                    },
                    /* legend: { display: false },*/
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
                },

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
        //console.log("ARRAY:", arr);

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

        //console.log("Gauge Data:", GaugeData);

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
                ctx.rotate(angle);
                ctx.beginPath();
                ctx.moveTo(0, -10);
                ctx.lineTo(height - (ctx.canvas.offsetTop + 50), 0);
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
                //ctx.fillText(needleValue + '%', cx + 20, cy - 90);
                ctx.fillText(needleValue + '%', cx + 50, cy - 50);
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

    var temp = jQuery.extend(true, {}, config);
    temp.type = newType;
    myChart = new Chart(ctx, temp);
};

function createdynamictable(yourdata, chartname, id, tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber) {
    checkTabularValue(tableheadercolor, tableheaderfontcolor, tableheaderfontname, tableheaderfontsize, tableheaderbold, tableheaderitalic, tableheaderunderline, tableheaderalign, showheader, showheaderbordershadow, tablecellfontcolor, tablecellfontname, tablecellfontsize, tablecellbold, tablecellitalic, tablecellunderline, tablecellalign, tablecellbordercolor, oddcellcolor, evencellcolor, showpagination, showlayoutbordershadow, showwraptext, showcompactnumber);
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
    //colTypes = colType;

    // CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE.
    var thead = table.createTHead();
    var tr = '';
    tr = thead.insertRow(0);
    for (var i = 0; i < col.length; i++) {
        var th = document.createElement("th");
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

    $('#' + tablename + ' thead').css("background-color", TableHeaderColor);
    $('#' + tablename + ' thead tr th').css("color", TableHeaderFontColor);
    $('#' + tablename + ' thead tr th').css("font-weight", TableHeaderBold);
    $('#' + tablename + ' thead tr th').css("font-style", TableHeaderItalic);
    $('#' + tablename + ' thead tr th').css("text-decoration", TableHeaderUnderline);
    $('#' + tablename + ' thead tr th').css("font-family", TableHeaderFontName);
    $('#' + tablename + ' thead tr th').css("font-size", TableHeaderFontSize + 'px');

    var tableDef = $('#' + tablename).DataTable(getDataTableDef(columns));
    tableDef.$('td').css('color', TableCellFontColor);
    tableDef.$('td').css("fontFamily", TableCellFontName);
    tableDef.$('td').css("font-weight", TableCellBold);
    tableDef.$('td').css("font-style", TableCellItalic);
    tableDef.$('td').css("text-decoration", TableCellUnderline);
    tableDef.$('td').css('font-size', TableCellFontSize + 'px');
    tableDef.$('td').css('border-color', TableCellBorderColor);
    //tableDef.$('tr.odd').css('background-color', OddCellColor);
    //tableDef.$('tr.even').css('background-color', EvenCellColor);
    //tableDef.$('td:nth-child' + '(' + columnIndex + ')').css("text-align", TableCellAlign);

}
function getDataTableDef(columnDef) {
    var datatableFormat = {
        columns: columnDef,
        "paging": ShowPagination,
        //"scrollY": "220px",
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
function reinitializeVariable() {
    //cardColor = '#f4f6f9';
    cardColor = '#fff';
    //headingText = '';
    headingColor = '#000';
    headingFontName = FontName;
    headingFontSize = FontSize;
    headingBold = 'normal';
    headingItalic = 'normal';
    headingUnderline = 'normal';
    headingAlign = 'left';
    IconShow = Boolean(false);
    TableHeaderColor = '';
    TableHeaderFontColor = '#000';
    TableHeaderFontName = FontName;
    TableHeaderFontSize = '10';
    TableHeaderBold = 'normal';
    TableHeaderItalic = 'normal';
    TableHeaderUnderline = 'normal';
    TableHeaderAlign = 'left';
    showHeader = Boolean(true);
    showHeaderBorderShadow = Boolean(false);
    TableCellFontColor = '#000';
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

function createdynamictable1(yourdata, chartname, id) {

    var tablename = '#mytable' + id;
    var parsedata = JSON.parse(yourdata);
    var table = document.createElement("table");
    table.setAttribute("id", tablename);
    table.classList.add('table');

    for (var i = 0; i <= parsedata.length; i++) {
        tr = table.insertRow(-1);

        for (var key in parsedata[0]) {
            if (i == 0) {
                //Inserting columns fields to the table
                var trCell = tr.insertCell(-1);
                trCell.innerHTML = key;
            }
            else {
                //Inserting table values to the table
                var trCell = tr.insertCell(-1);
                trCell.innerHTML = parsedata[i - 1][key];
            }
        };

    }


    var div = document.getElementById(chartname);
    div.innerHTML = "";
    div.appendChild(table);

    $(function () {
        $(tablename).DataTable({
            "responsive": true, "lengthChange": false, "autoWidth": false
        });
    });



}
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
    $(eleName).css("position", "absolute");
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
    //$(eleName).css("position", "absolute");
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