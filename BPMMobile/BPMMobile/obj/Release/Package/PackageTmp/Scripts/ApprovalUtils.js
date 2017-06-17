


//提交,审批,加签,已阅
function PostXml(xml) {
    $.ajax({
        type: "POST",
        url: "/api/bpm/PostProcess",
        data: { '': xml },
        beforeSend: function (XHR) {
            XHR.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('ticket'));

        },
        success: function (data, status) {

            if (status == "success") {
                //console.log(data);
                mui.toast("提交给" + data.Recipients[0].Recipient.DisplayName);
                setTimeout("window.location.href = '/Pages/index.html?ticket=" + localStorage.getItem('ticket') + "'", 2000);

            } else {
                mui.toast("提交失败!请稍后重试");
            }
        },
        error: function () {


        },
        complete: function () {

        }

    });

}

//知会
var noticeOpenIdArr;
function Notify() {
    XuntongJSBridge.call('createPop', {
        'popTitle': '',
        'popTitleCallBackId': '1',
        'items': [
            { 'text': '知会', 'callBackId': 'callback1' },

        ], 'menuList': [
            'refresh', 'openWithBrowser'
        ],

    }, function (result) {
        if (result.success == true || result.success == 'true') {
            var callBackId = result.data ? result.data.callBackId : '';
            if (callBackId == 'callback1') {

                XuntongJSBridge.call('selectPersons', { 'isMulti': 'true', 'pType': '1' }, function (result) {


                    noticeOpenIdArr = new Array();
                    if (result.success == true) {

                        for (var i = 0; i < result.data.persons.length; i++) {

                            noticeOpenIdArr.push(result.data.persons[i].openId);

                        }
                        $("#noticeOpenId").val(noticeOpenIdArr);
                        //知会

                        $.ajax({
                            type: "POST",
                            url: "/api/bpm/PostAccount",
                            data: { "ids": noticeOpenIdArr },
                            beforeSend: function (XHR) {
                                XHR.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('ticket'));

                            }, success: function (data, status) {

                                if (status == "success") {
                                    var accounts = new Array();
                                    var accName = new Array();
                                    for (var i = 0; i < data.data.length; i++) {
                                        accounts.push((String)(data.data[i].phone));
                                        accName.push(data.data[i].name);
                                    }


                                    var btnArry = ["取消", "确定"];
                                    mui.confirm('将知会下列人员:' + accName, '知会通知', btnArry, function (e) {
                                        if (e.index == 1) {
                                            $.ajax({
                                                type: "POST",
                                                url: "/api/bpm/PostInform",
                                                data: { 'taskID': taskId, 'comments': '', 'accounts': accounts },
                                                dataType: "json",
                                                beforeSend: function (XHR) {

                                                    XHR.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('ticket'));

                                                },
                                                success: function (data, status) {

                                                    if (status == "success") {

                                                    }
                                                }, error: function (e) {
                                                    //console.log(e);  

                                                },
                                                complete: function () { }


                                            });
                                        } else {

                                        }
                                    });


                                }
                            }, error: function (e) {

                            }, complete: function () { }


                        });


                    }

                });


            } else {

            }
        }

    });

}

//获取当前日期
function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var hour = date.getHours();
    if (hour >= 0 && hour <= 9) {
        hour = "0" + hour;
    }
    var minute = date.getMinutes();
    if (minute >= 0 && minute <= 9) {
        minute = "0" + minute;
    }
    var currentdate = month + seperator1 + strDate
        + " " + hour + seperator2 + minute
        ;
    return currentdate;
}

//格式化时间为MM-dd hh:mm
function FormatterTime(time) {
    var time = String(time);
    var ymd = time.substring(time.indexOf("-") + 1, time.indexOf("T"));
    var hms = time.substring(time.indexOf("T") + 1, time.lastIndexOf(":"));
    time = ymd + " " + hms;

    return time;
}


//退回重填
function Refilled(taskId) {


    var comments;
    var btnArray = ['取消', '确定'];
    mui.prompt('请输入原因：', '', '退回理由', btnArray, function (e) {
        if (e.index == 1) {
            comments = e.value;

            $.ajax({
                type: "POST",
                url: "/api/bpm/PostRecedeRestart",
                data: { 'taskID': taskId, 'comments': comments },
                beforeSend: function (XHR) {
                    XHR.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('ticket'));

                },
                success: function (data, status) {

                    if (status == "success") {

                        mui.toast("操作成功");
                        setTimeout(" window.location.href = '/Pages/BaseFlow/UndoFlow.html'", 2000);
                    } else {
                        mui.toast("操作失败,请稍后重试");
                    }
                },
                error: function (e) {
                    //console.log(e);  

                },
                complete: function () { }

            });
        } else {

        }
    }, 'div');

}

//获取BPM参数
var BPMOU;
function getBPMParam() {
    $.ajax({

        type: 'get',
        url: "/api/bpm/GetBPMParam",
        data: {},
        beforeSend: function (XHR) {
            XHR.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('ticket'));
        },
        success: function (data, status) {
            if (status == "success") {
                //console.log(data);
                BPMOU = data.Position[0].FullName;
            }
        },
        error: function (e) {
        },

        complete: function () { }

    });

}

//前端获取ticket
function getUrlParam(name) {

    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");

    var r;
    if (window.location.search != null && window.location.search != "") {

        r = window.location.search.substr(1).match(reg);

        if (r != null) {

            return unescape(r[2]);
        }
    } else {
        var ticket = localStorage.getItem('ticket');

        return ticket;
    }
    return null;
}

//拒绝
function reject() {
    var comments;
    var btnArray = ['取消', '确定'];
    mui.prompt('请输入原因：', '', '拒绝理由', btnArray, function (e) {
        if (e.index == 1) {
            comments = e.value;



            $.ajax({
                type: "POST",
                url: "/api/bpm/PostReject",
                data: { 'taskID': $("#taskId").val(), 'comments': comments },
                beforeSend: function (XHR) {
                    XHR.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('ticket'));

                },
                success: function (data, status) {

                    if (status == "success") {

                        mui.toast("操作成功");
                        setTimeout(" window.location.href = '/Pages/BaseFlow/UndoFlow.html'", 2000);
                    } else {
                        mui.toast("操作失败,请稍后重试");
                    }
                },
                error: function (e) {
                    console.log(e);

                },
                complete: function () { }

            });
        } else {

        }
    }, 'div');

}




