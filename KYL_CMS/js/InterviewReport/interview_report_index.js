var InterviewReportIndex = {
    Action: null,
    INTERVIEW: null,

    Page_Init: function () {
        this.EventBinding();
        InterviewReportIndex.OptionRetrieve();
        InterviewReportIndex.ActionSwitch('R');
        InterviewReportIndex.DateRangePickerBind($('#DATE_RANGE'));

        var dt = new Date();
        var today = dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate() ;
        $('#from').val(today);
        $('#to').val(today);
    },

    EventBinding: function () {
        $('#query').click(function () {
            InterviewReportIndex.InterviewRetrieve();
        });

        $('#page_number, #page_size').change(function () {
                InterviewReportIndex.InterviewRetrieve();
        });
        
        $('#excel').click(function () {
            InterviewReportIndex.InterviewExcel();
        });

    },

    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#excel').show();
            $('#section_retrieve').show();
        }
        this.Action = action;
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            TableItem: ['userName'],
            PhraseGroup: ['page_size', 'SPECIAL_IDENTITY', 'INTERVIEW_CLASSIFY', 'FEELING', 'INTERVENTION']
        };

        $.ajax({
            async: false,
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                console.log(data);
                var response = JSON.parse(data);
                //console.log(response);
                if (response.ReturnStatus.Code === 0) {

                    $.each(response.ItemList.page_size, function (idx, row) {
                        $('#page_size').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    ////特殊身份別
                    //$.each(response.ItemList.SPECIAL_IDENTITY, function (idx, row) {
                    //    var template = `
                    //        <div class="form-group col-12 col-sm-6 col-md-6 col-lg-4 col-xl-4 " >
                    //            <label><input type="checkbox" class="fields" name="SPECIAL_IDENTITY" value="`+ row.Key + `">` + row.Key + ` ` + row.Value + `</label>
                    //            <input type="text" name="SPECIAL_IDENTITY`+ row.Key + `" class="form-control form-control-sm fields">
                    //        </div>
                    //    `;
                    //    var temp = $detail_template.find('[name=special_identity]').append(template);
                    //});

                    ////來電者主述問題分類之圈選
                    //$.each(response.ItemList.INTERVIEW_CLASSIFY, function (idx, row) {
                    //    var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                    //    if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    //    lbl.append('<input class="form-check-input fields" type="checkbox" name="INTERVIEW_CLASSIFY" value="' + row.Key + '">' + row.Key + ' ' + row.Value);
                    //    var temp = $detail_template.find('[name=interview_classify]').append(lbl);
                    //});

                    ////案主在此困擾的情緒
                    //$.each(response.ItemList.FEELING, function (idx, row) {
                    //    var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                    //    if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    //    lbl.append('<input class="form-check-input fields" type="checkbox" name="FEELING" value="' + row.Key + '">' + row.Key + ' ' + row.Value);
                    //    var temp = $detail_template.find('[name=feeling]').append(lbl);
                    //});

                    ////接案過程介入方式
                    //$.each(response.ItemList.INTERVENTION, function (idx, row) {
                    //    var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                    //    if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    //    lbl.append('<input class="form-check-input fields" type="checkbox" name="INTERVENTION" value="' + row.Key + '">' + row.Key + ' ' + row.Value);
                    //    var temp = $detail_template.find('[name=intervention]').append(lbl);
                    //});
                }
                else {
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易代碼:' + response.ReturnStatus.Code + '<br/>交易說明:' + response.ReturnStatus.Message + '</p>');
                    $('#modal').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('' + xhr.status + ';' + ajaxOptions + ';' + thrownError);
            },
            complete: function (xhr, status) {
                //alert('' + xhr.status + ';' + status );
            }
        });
    },

    InterviewRetrieve: function () {
        var url = 'InterviewRetrieve';
        var request = {
            INTERVIEW: {
				SN: $('#section_retrieve #SN').val(),
				VOLUNTEER_SN: $('#section_retrieve #VOLUNTEER_SN').val(),
				INCOMING_DATE: $('#section_retrieve #INCOMING_DATE').val(),
				DURING: $('#section_retrieve #DURING').val(),
				CASE_NO: $('#section_retrieve #CASE_NO').val(),
				NAME: $('#section_retrieve #NAME').val(),
				TEL: $('#section_retrieve #TEL').val(),
				CONTACT_TIME: $('#section_retrieve #CONTACT_TIME').val(),
				TREATMENT: $('#section_retrieve #TREATMENT').val(),
				TREATMENT_MEMO: $('#section_retrieve #TREATMENT_MEMO').val(),
				CASE_SOURCE: $('#section_retrieve #CASE_SOURCE').val(),
				CASE_SOURCE_MEMO: $('#section_retrieve #CASE_SOURCE_MEMO').val(),
				GENDER: $('#section_retrieve #GENDER').val(),
				AGE: $('#section_retrieve #AGE').val(),
				EDUCATION: $('#section_retrieve #EDUCATION').val(),
				CAREER: $('#section_retrieve #CAREER').val(),
				CAREER_MEMO: $('#section_retrieve #CAREER_MEMO').val(),
				CITY: $('#section_retrieve #CITY').val(),
				MARRIAGE: $('#section_retrieve #MARRIAGE').val(),
				PHYSIOLOGY: $('#section_retrieve #PHYSIOLOGY').val(),
				PHYSIOLOGY_MEMO: $('#section_retrieve #PHYSIOLOGY_MEMO').val(),
				PSYCHOLOGY: $('#section_retrieve #PSYCHOLOGY').val(),
				PSYCHOLOGY_MEMO: $('#section_retrieve #PSYCHOLOGY_MEMO').val(),
				VISITED: $('#section_retrieve #VISITED').val(),
				FAMILY: $('#section_retrieve #FAMILY').val(),
				EXPERIENCE: $('#section_retrieve #EXPERIENCE').val(),
				HARASS: $('#section_retrieve #HARASS').val(),
				SOLVE_DEGREE: $('#section_retrieve #SOLVE_DEGREE').val(),
				ABOUT_SELF: $('#section_retrieve #ABOUT_SELF').val(),
				ABOUT_OTHERS: $('#section_retrieve #ABOUT_OTHERS').val(),
				BEHAVIOR: $('#section_retrieve #BEHAVIOR').val(),
				ADDITION: $('#section_retrieve #ADDITION').val(),
				INNER_DEMAND: $('#section_retrieve #INNER_DEMAND').val(),
				OPINION: $('#section_retrieve #OPINION').val(),
				CASE_STATUS: $('#section_retrieve #CASE_STATUS').val(),
				CDATE: $('#section_retrieve #CDATE').val(),
				CUSER: $('#section_retrieve #CUSER').val(),
				MDATE: $('#section_retrieve #MDATE').val(),
				MUSER: $('#section_retrieve #MUSER').val(),
            },
            PageNumber: $('#page_number').val() ? $('#page_number').val() : 1,
            PageSize: $('#page_size').val() ? $('#page_size').val() : 1
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 0) {
                    $('#gridview >  tbody').html('');
                    $('#rows_count').text(response.Pagination.RowCount);
                    $('#interval').text(response.Pagination.MinNumber + '-' + response.Pagination.MaxNumber);
                    $('#page_number option').remove();
                    for (var i = 1; i <= response.Pagination.PageCount; i++) {
                        $('#page_number').append($('<option></option>').attr('value', i).text(i));
                    }
                    $('#page_number').val(response.Pagination.PageNumber);
                    $('#page_count').text(response.Pagination.PageCount);
                    $('#time_consuming').text((Date.parse(response.Pagination.EndTime) - Date.parse(response.Pagination.StartTime)) / 1000);

                    var htmlRow = '';
                    if (response.Pagination.RowCount > 0) {
                        $.each(response.INTERVIEW, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="InterviewReportIndex.RowSelected(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
							htmlRow += '<td>' + row.SN + '</td>';
							htmlRow += '<td>' + row.VOLUNTEER_SN + '</td>';
							htmlRow += '<td>' + row.INCOMING_DATE + '</td>';
							htmlRow += '<td>' + row.DURING + '</td>';
							htmlRow += '<td>' + row.CASE_NO + '</td>';
							htmlRow += '<td>' + row.NAME + '</td>';
							htmlRow += '<td>' + row.TEL + '</td>';
							htmlRow += '<td>' + row.CONTACT_TIME + '</td>';
							htmlRow += '<td>' + row.TREATMENT + '</td>';
							htmlRow += '<td>' + row.TREATMENT_MEMO + '</td>';
							htmlRow += '<td>' + row.CASE_SOURCE + '</td>';
							htmlRow += '<td>' + row.CASE_SOURCE_MEMO + '</td>';
							htmlRow += '<td>' + row.GENDER + '</td>';
							htmlRow += '<td>' + row.AGE + '</td>';
							htmlRow += '<td>' + row.EDUCATION + '</td>';
							htmlRow += '<td>' + row.CAREER + '</td>';
							htmlRow += '<td>' + row.CAREER_MEMO + '</td>';
							htmlRow += '<td>' + row.CITY + '</td>';
							htmlRow += '<td>' + row.MARRIAGE + '</td>';
							htmlRow += '<td>' + row.PHYSIOLOGY + '</td>';
							htmlRow += '<td>' + row.PHYSIOLOGY_MEMO + '</td>';
							htmlRow += '<td>' + row.PSYCHOLOGY + '</td>';
							htmlRow += '<td>' + row.PSYCHOLOGY_MEMO + '</td>';
							htmlRow += '<td>' + row.VISITED + '</td>';
							htmlRow += '<td>' + row.FAMILY + '</td>';
							htmlRow += '<td>' + row.EXPERIENCE + '</td>';
							htmlRow += '<td>' + row.HARASS + '</td>';
							htmlRow += '<td>' + row.SOLVE_DEGREE + '</td>';
							htmlRow += '<td>' + row.ABOUT_SELF + '</td>';
							htmlRow += '<td>' + row.ABOUT_OTHERS + '</td>';
							htmlRow += '<td>' + row.BEHAVIOR + '</td>';
							htmlRow += '<td>' + row.ADDITION + '</td>';
							htmlRow += '<td>' + row.INNER_DEMAND + '</td>';
							htmlRow += '<td>' + row.OPINION + '</td>';
							htmlRow += '<td>' + row.CASE_STATUS + '</td>';
							htmlRow += '<td>' + row.CDATE + '</td>';
							htmlRow += '<td>' + row.CUSER + '</td>';
							htmlRow += '<td>' + row.MDATE + '</td>';
							htmlRow += '<td>' + row.MUSER + '</td>';
                            htmlRow += '</tr>';
                            $('#gridview >  tbody').append(htmlRow);
                        });
                    }
                    else {
                        htmlRow = '<tr><td colspan="12" style="text-align:center">data not found</td></tr>';
                        $('#gridview >  tbody').append(htmlRow);
                    }
                }
                else {
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易代碼:' + response.ReturnStatus.Code + '<br/>交易說明:' + response.ReturnStatus.Message + '</p>');
                    $('#modal').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#modal .modal-title').text(ajaxOptions);
                $('#modal .modal-body').html('<p>' + xhr.status + ' ' + thrownError + '</p>');
                $('#modal').modal('show');
            },
            complete: function (xhr, status) {
            }
        });
    },

    InterviewExcel: function () {
        
        var url = '/InterviewReport/InterviewReportExcel';
        
        var request = {
            Start: $('#from').val(),
            End: $('#to').val()
        };
        console.log(JSON.stringify(request));
        window.location.href = url + '?json=' + JSON.stringify(request);
    },

    DataValidate: function () {
        $('#section_modify').validate({
            rules: {
				SN: 'required',
				VOLUNTEER_SN: 'required',
				INCOMING_DATE: 'required',
				DURING: 'required',
				CASE_NO: 'required',
				NAME: 'required',
				TEL: 'required',
				CONTACT_TIME: 'required',
				TREATMENT: 'required',
				TREATMENT_MEMO: 'required',
				CASE_SOURCE: 'required',
				CASE_SOURCE_MEMO: 'required',
				GENDER: 'required',
				AGE: 'required',
				EDUCATION: 'required',
				CAREER: 'required',
				CAREER_MEMO: 'required',
				CITY: 'required',
				MARRIAGE: 'required',
				PHYSIOLOGY: 'required',
				PHYSIOLOGY_MEMO: 'required',
				PSYCHOLOGY: 'required',
				PSYCHOLOGY_MEMO: 'required',
				VISITED: 'required',
				FAMILY: 'required',
				EXPERIENCE: 'required',
				HARASS: 'required',
				SOLVE_DEGREE: 'required',
				ABOUT_SELF: 'required',
				ABOUT_OTHERS: 'required',
				BEHAVIOR: 'required',
				ADDITION: 'required',
				INNER_DEMAND: 'required',
				OPINION: 'required',
				CASE_STATUS: 'required',
				CDATE: 'required',
				CUSER: 'required',
				MDATE: 'required',
				MUSER: 'required',
            }
        });
    },

    ValueRecover: function (action) {
        if (action == 'U') {
            if (InterviewReportIndex.INTERVIEW) {
                $('.modify').each(function (index, value) {
                    $(value).val(InterviewReportIndex.INTERVIEW[value.id]);
                });
            }
        }
        else {
            $('.modify').each(function (index, value) {
                $(value).val('');
            });
        }
    },

    DateRangePickerBind: function (obj) {
        moment.locale('zh-tw');
        ////$('input[name="INCOMING_DATE"]').daterangepicker();
        //$('input[name="INCOMING_DATE"]').daterangepicker({
        //    singleDatePicker: true,
        //    showDropdowns: true,
        //    timePicker: true,
        //    timePicker24Hour: true,
        //    locale: { format: 'YYYY/MM/DD HH:mm', applyLabel: '確認', cancelLabel: '取消' }
        //}, function (start, end, label) {
        //    //var years = moment().diff(start, 'years');
        //    //alert("You are " + years + " years old!");
        //});


        obj.daterangepicker({
            showDropdowns: true,
            locale: { format: 'YYYY-MM-DD', applyLabel: '確認', cancelLabel: '取消' }
        }, function (start, end, label) {
            $('#from').val(start.format('YYYY-MM-DD'))
            $('#to').val(end.format('YYYY-MM-DD'))
            //var years = moment().diff(start, 'years');
            //alert("You are " + years + " years old!");
        });


    }
};
