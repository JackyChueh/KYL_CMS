var HistoryIndex = {
    UserId:null,
    Action: null,
    CASE: null,
    INTERVIEW: [],
    //Option: null,
    QueryStartDate: null,
    QueryEndDate: null,

    Page_Init: function () {
        HistoryIndex.EventBinding();
        HistoryIndex.OptionRetrieve()
        HistoryIndex.ActionSwitch('R');

        HistoryIndex.HistoryRetrieve();
    },

    EventBinding: function () {
        $('#query').click(function () {
            HistoryIndex.HistoryRetrieve();
        });

        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                HistoryIndex.HistoryRetrieve();
            }
        });


        $('#return').click(function () {
            HistoryIndex.ActionSwitch('R');
            HistoryIndex.ValueRecover();
            HistoryIndex.HistoryRetrieve();
        });

    },


    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#section_retrieve').show();
        } else if (action === 'U') {
            $('#return').show();
            $('#section_modify').show();
        } else if (action === 'C') {
            $('#return').show();
            $('#section_modify').show();
        }
        HistoryIndex.Action = action;

    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            TableItem: ['userName'],
            PhraseGroup: ['page_size', 'CONTACT_TIME', 'TREATMENT', 'CASE_SOURCE', 'GENDER', 'AGE', 'EDUCATION', 'CAREER', 'CITY', 'SPECIAL_IDENTITY', 'MARRIAGE', 'PHYSIOLOGY', 'PSYCHOLOGY', 'VISITED', 'INTERVIEW_CLASSIFY', 'SOLVE_DEGREE', 'FEELING', 'INTERVENTION', 'CASE_STATUS']
        };

        $.ajax({
            async: false,
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                //console.log(data);
                var response = JSON.parse(data);
                //console.log(response);
                if (response.ReturnStatus.Code === 0) {

                    //$('#page_size').append('<option value=""></option>');
                    $.each(response.ItemList.page_size, function (idx, row) {
                        $('#page_size').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    var section_retrieve = $('#section_retrieve');
                    var obj = null;

                    obj = section_retrieve.find('[name=CASE_STATUS]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CASE_STATUS, function (idx, row) {
                        if ('01,03'.indexOf(row.Key) > -1) {
                            var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                            if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        }
                    });

                    obj = section_retrieve.find('[name=CONTACT_TIME]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CONTACT_TIME, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    var $detail_template = $('#detail_template');
                    var obj = null;

                    obj = $detail_template.find('[name=VOLUNTEER_SN]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.userName, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CONTACT_TIME]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CONTACT_TIME, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=TREATMENT]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.TREATMENT, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CASE_SOURCE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CASE_SOURCE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=GENDER]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.GENDER, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=AGE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.AGE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=EDUCATION]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.EDUCATION, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CAREER]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CAREER, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CITY]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CITY, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=MARRIAGE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.MARRIAGE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=PHYSIOLOGY]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.PHYSIOLOGY, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=PSYCHOLOGY]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.PSYCHOLOGY, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=VISITED]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.VISITED, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    //特殊身份別
                    $.each(response.ItemList.SPECIAL_IDENTITY, function (idx, row) {
                        var template = `
                            <div class="form-group col-12 col-sm-6 col-md-6 col-lg-4 col-xl-4 " >
                                <label><input type="checkbox" class="fields" name="SPECIAL_IDENTITY" value="`+ row.Key + `" disabled>` + row.Key + ` ` + row.Value + `</label>
                                <input type="text" name="SPECIAL_IDENTITY`+ row.Key + `" class="form-control form-control-sm fields" disabled>
                            </div>
                        `;
                        var temp = $detail_template.find('[name=special_identity]').append(template);
                    });

                    //來電者主述問題分類之圈選
                    $.each(response.ItemList.INTERVIEW_CLASSIFY, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" type="checkbox" name="INTERVIEW_CLASSIFY" value="' + row.Key + '" disabled>' + row.Key + ' ' + row.Value);
                        var temp = $detail_template.find('[name=interview_classify]').append(lbl);
                    });

                    //案主解決問題的程度
                    obj = $detail_template.find('[name=SOLVE_DEGREE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.SOLVE_DEGREE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    //案主在此困擾的情緒
                    $.each(response.ItemList.FEELING, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" type="checkbox" name="FEELING" value="' + row.Key + '" disabled>' + row.Key + ' ' + row.Value);
                        var temp = $detail_template.find('[name=feeling]').append(lbl);
                    });

                    //接案過程介入方式
                    $.each(response.ItemList.INTERVENTION, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" type="checkbox" name="INTERVENTION" value="' + row.Key + '" disabled>' + row.Key + ' ' + row.Value);
                        var temp = $detail_template.find('[name=intervention]').append(lbl);
                    });

                    //完成進度
                    $.each(response.ItemList.CASE_STATUS, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" type="radio" name="CASE_STATUS" value="' + row.Key + '" disabled>' + row.Value);
                        var temp = $detail_template.find('[name=case_status]').append(lbl);
                    });
                    obj = $detail_template.find('[name=case_status]').find('input');
                    $(obj[0]).prop('checked', true);

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

    HistoryRetrieve: function () {
        var url = 'HistoryRetrieve';
        var request = {
            INTERVIEW: {
                NAME: $('#section_retrieve #NAME').val()
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
                    var temp = '';
                    if (response.Pagination.RowCount > 0) {
                        $.each(response.INTERVIEW, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="HistoryIndex.RowSelected(' + row.CASE_SN + ');" data-toggle="tooltip" data-placement="right" title="修改" style="cursor: pointer;"></a></td>';
                            htmlRow += '<td>' + row.CASE_NO + '</td>';
                            htmlRow += '<td>' + row.DURING + '</td>';
                            htmlRow += '<td>' + row.NAME + '</td>';
                            htmlRow += '<td>' + row.OPINION + '</td>';
                            htmlRow += '<td>' + row.HARASS + '</td>';
                            temp = row.CASE_STATUS === '01' ? '草稿存檔' : row.CASE_STATUS === '02' ? '送交審核' : row.CASE_STATUS === '03' ? '審核退回請修改' : row.CASE_STATUS === '04' ? '審核完成' : '';
                            htmlRow += '<td>' + temp + '</td>';
                            htmlRow += '</tr>';
                            $('#gridview >  tbody').append(htmlRow);
                        });
                    }
                    else {
                        htmlRow = '<tr><td colspan="13" style="text-align:center">data not found</td></tr>';
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

    CaseQuery: function (SN) {
        var url = 'HistoryQuery';
        var request = {
            CASE: {
                SN: SN,
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                //console.log(data);
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 0) {
                    if (response.CASE) {
                        var master_sectoin = $('#master_sectoin');
                        master_sectoin.find('[name=SN]').val(response.CASE.SN);
                        master_sectoin.find('[name=NAME1]').val(response.CASE.NAME1);
                        master_sectoin.find('[name=NAME2]').val(response.CASE.NAME2);
                        master_sectoin.find('[name=NAME3]').val(response.CASE.NAME3);
                        master_sectoin.find('[name=NAME4]').val(response.CASE.NAME4);
                        master_sectoin.find('[name=NAME5]').val(response.CASE.NAME5);
                        master_sectoin.find('[name=NAME6]').val(response.CASE.NAME6);
                        master_sectoin.find('[name=TEL1]').val(response.CASE.TEL1);
                        master_sectoin.find('[name=TEL2]').val(response.CASE.TEL2);
                        master_sectoin.find('[name=TEL3]').val(response.CASE.TEL3);
                        master_sectoin.find('[name=TEL4]').val(response.CASE.TEL4);
                        master_sectoin.find('[name=TEL5]').val(response.CASE.TEL5);
                        master_sectoin.find('[name=TEL6]').val(response.CASE.TEL6);
                        master_sectoin.find('[name=YEARS_OLD]').val(response.CASE.YEARS_OLD);
                        master_sectoin.find('[name=ADDRESS]').val(response.CASE.ADDRESS);
                        master_sectoin.find('[name=TEACHER1]').val(response.CASE.TEACHER1);
                        master_sectoin.find('[name=TEACHER2]').val(response.CASE.TEACHER2);
                        master_sectoin.find('[name=TEACHER3]').val(response.CASE.TEACHER3);
                        master_sectoin.find('[name=FAMILY]').val(response.CASE.FAMILY);
                        if (response.CASE.FAMILY_FILE) {
                            master_sectoin.find('[name=FAMILY_FILE]').attr('src', '/img/upload/' + response.CASE.FAMILY_FILE);
                        }
                        else {
                            master_sectoin.find('[name=FAMILY_FILE]').attr('src', '');
                        }
                        master_sectoin.find('[name=RESIDENCE]').val(response.CASE.RESIDENCE);
                        master_sectoin.find('[name=NOTE]').val(response.CASE.NOTE);
                        master_sectoin.find('[name=PROBLEM]').val(response.CASE.PROBLEM);
                        master_sectoin.find('[name=EXPERIENCE]').val(response.CASE.EXPERIENCE);
                        master_sectoin.find('[name=SUGGEST]').val(response.CASE.SUGGEST);
                        master_sectoin.find('[name=MERGE_REASON]').val(response.CASE.MERGE_REASON);
                        master_sectoin.find('[name=CDATE]').val(response.CASE.CDATE.replace('T', ' '));
                        master_sectoin.find('[name=CUSER]').val(response.CASE.CUSER);
                        master_sectoin.find('[name=MDATE]').val(response.CASE.MDATE.replace('T', ' '));
                        master_sectoin.find('[name=MUSER]').val(response.CASE.MUSER);

                        HistoryIndex.INTERVIEW = response.CASE.CASE_DETAIL
                        HistoryIndex.DetailRefresh();
                        HistoryIndex.CASE = response.CASE;
                        //$('#section_modify').validate();
                    }
                    HistoryIndex.ActionSwitch('U');
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

    RowSelected: function (key) {
        HistoryIndex.CaseQuery(key);
    },

    DataValidate: function () {
        var master_sectoin = $('#master_sectoin');
        var ErrMsg = '';
        if (master_sectoin) {
            if (!(master_sectoin.find('input[name=NAME1]').val() ||
                master_sectoin.find('input[name=NAME2]').val() ||
                master_sectoin.find('input[name=NAME3]').val() ||
                master_sectoin.find('input[name=NAME4]').val() ||
                master_sectoin.find('input[name=NAME5]').val() ||
                master_sectoin.find('input[name=NAME6]').val())) {
                ErrMsg += '稱謂1~6至少填入一個<br/>';
            }

            if (!(master_sectoin.find('input[name=TEL1]').val() ||
                master_sectoin.find('input[name=TEL2]').val() ||
                master_sectoin.find('input[name=TEL3]').val() ||
                master_sectoin.find('input[name=TEL4]').val() ||
                master_sectoin.find('input[name=TEL5]').val() ||
                master_sectoin.find('input[name=TEL6]').val())) {
                ErrMsg += '電話1~6至少填入一個<br/>';
            }

            if (ErrMsg) {
                $('#modal .modal-title').text('欄位內容檢核');
                $('#modal .modal-body').html('<p>' + ErrMsg + '</p>');
                $('#modal').modal('show');
                return false;
            }
            else {
                return true;
            }
        }
    },

    DetailRefresh: function () {
        if (HistoryIndex.INTERVIEW.length > 0) {
            $('#detail_gridview >  tbody').html('');
            var htmlRow = '';
            var temp = null;
            $.each(HistoryIndex.INTERVIEW, function (idx, row) {
                if (row.DataRow !== 'delete') {
                    temp = '';
                    htmlRow = '<tr>';
                    htmlRow += '<td style="text-align:center"><a class="fa fa-edit fa-lg" onclick="HistoryIndex.DetailSelected(' + idx + ');" data-toggle="tooltip" data-placement="right" title="修改" style="cursor: pointer;"></a></td>';
                    htmlRow += '<td>' + row.CASE_NO + '</td>';
                    htmlRow += '<td>' + row.INCOMING_DATE.substr(0, 16).replace('T', ' ') + '</td>';
                    htmlRow += '<td>' + row.DURING + '</td>';
                    htmlRow += '<td>' + row.VOLUNTEER_SN + '</td>';
                    if (row.INTERVIEW_CLASSIFY) {
                        $.each(row.INTERVIEW_CLASSIFY, function (idx2, item) {
                            temp += item.PHRASE_KEY ? ', ' + item.PHRASE_KEY : '';
                        });
                    }
                    temp = temp.substr(1);
                    htmlRow += '<td>' + temp + '</td>';
                    temp = row.CASE_STATUS === '01' ? '草稿存檔' : row.CASE_STATUS === '02' ? '送交審核' : row.CASE_STATUS === '03' ? '審核退回請修改' : row.CASE_STATUS === '04' ? '審核完成' : '';
                    htmlRow += '<td>' + temp + '</td>';
                    htmlRow += '</tr>';
                    $('#detail_gridview >  tbody').append(htmlRow);
                }
            });
        }
        else {
            $('#detail_gridview >  tbody').html('');
            htmlRow = '<tr><td colspan="12" style="text-align:center">data not found</td></tr>';
            $('#detail_gridview >  tbody').append(htmlRow);
        }
    },

    DetailSelected: function (idx) {
        var template = $('#detail_template').clone();
        template.prop('id', 'detail_interview');
        template.attr('data-row', 'edit');
        template.attr('data-index', idx);

        //控制邏輯
        var treatment = template.find('select[name=TREATMENT]');
        treatment.bind("change", function () {
            if ('04,05,06'.indexOf(treatment.val()) > -1) {
                template.find('select[name=CASE_SOURCE]').val('06');
                template.find('select[name=GENDER]').val('04');
                template.find('select[name=AGE]').val('08');
                template.find('select[name=EDUCATION]').val('08');
                template.find('select[name=CAREER]').val('14');
                template.find('select[name=CITY]').val('19');
                template.find('select[name=MARRIAGE]').val('03');
                template.find('select[name=PHYSIOLOGY]').val('01');
                template.find('select[name=PSYCHOLOGY]').val('01');
                template.find('select[name=VISITED]').val('04');
                special_identity = template.find('div[name=special_identity]');
                special_identity.find('input[type=checkbox][value=01]').prop('checked', true);
                interview_classify = template.find('div[name=interview_classify]');
                interview_classify.find('input[type=checkbox][value=U1]').prop('checked', true);
            }
        });

        var value = HistoryIndex.INTERVIEW[idx];
        template.find('input[name=SN]').val(value.SN);
        template.find('select[name=VOLUNTEER_SN]').val(value.VOLUNTEER_SN);
        template.find('input[name=INCOMING_DATE]').val(value.INCOMING_DATE.substr(0, 16).replace('T', ' '));
        //HistoryIndex.DateRangePickerBind(template.find('input[name=INCOMING_DATE]'));
        template.find('input[name=DURING]').val(value.DURING);
        template.find('input[name=CASE_NO ]').val(value.CASE_NO);
        template.find('input[name=NAME]').val(value.NAME);
        template.find('input[name=TEL]').val(value.TEL);
        template.find('select[name=CONTACT_TIME]').val(value.CONTACT_TIME);
        template.find('select[name=TREATMENT]').val(value.TREATMENT);
        template.find('input[name=TREATMENT_MEMO]').val(value.TREATMENT_MEMO);
        template.find('select[name=CASE_SOURCE]').val(value.CASE_SOURCE);
        template.find('input[name=CASE_SOURCE_MEMO]').val(value.CASE_SOURCE_MEMO);
        template.find('select[name=GENDER]').val(value.GENDER);
        template.find('select[name=AGE]').val(value.AGE);
        template.find('select[name=EDUCATION]').val(value.EDUCATION);
        template.find('select[name=CAREER]').val(value.CAREER);
        template.find('input[name=CAREER_MEMO]').val(value.CAREER_MEMO);
        template.find('select[name=CITY]').val(value.CITY);
        template.find('select[name=MARRIAGE]').val(value.MARRIAGE);
        template.find('input[name=MARRIAGE_MEMO]').val(value.MARRIAGE_MEMO);
        template.find('select[name=PHYSIOLOGY]').val(value.PHYSIOLOGY);
        template.find('input[name=PHYSIOLOGY_MEMO]').val(value.PHYSIOLOGY_MEMO);
        template.find('select[name=PSYCHOLOGY]').val(value.PSYCHOLOGY);
        template.find('input[name=PSYCHOLOGY_MEMO]').val(value.PSYCHOLOGY_MEMO);
        template.find('select[name=VISITED]').val(value.VISITED);

        var special_identity = template.find('div[name=special_identity]');
        $.each(value.SPECIAL_IDENTITY, function (idx, val) {
            special_identity.find('input[type=checkbox][value=' + val.PHRASE_KEY + ']').prop('checked', true);
            special_identity.find('input[name=SPECIAL_IDENTITY' + val.PHRASE_KEY + ']').val(val.MEMO);
        });

        template.find('textarea[name=FAMILY]').val(value.FAMILY);
        template.find('textarea[name=EXPERIENCE]').val(value.EXPERIENCE);
        template.find('textarea[name=HARASS]').val(value.HARASS);

        var interview_classify = template.find('div[name=interview_classify]');
        $.each(value.INTERVIEW_CLASSIFY, function (index, value) {
            interview_classify.find('input[type=checkbox][value=' + value.PHRASE_KEY + ']').prop('checked', true);
        });

        template.find('select[name=SOLVE_DEGREE]').val(value.SOLVE_DEGREE);
        template.find('textarea[name=ABOUT_SELF]').val(value.ABOUT_SELF);
        template.find('textarea[name=ABOUT_OTHERS]').val(value.ABOUT_OTHERS);

        var feeling = template.find('div[name=feeling]');
        $.each(value.FEELING, function (index, value) {
            feeling.find('input[type=checkbox][value=' + value.PHRASE_KEY + ']').prop('checked', true);
        });
        template.find('input[name=FEELING_MEMO]').val(value.FEELING_MEMO);

        template.find('textarea[name=BEHAVIOR]').val(value.BEHAVIOR);
        template.find('textarea[name=INNER_DEMAND]').val(value.INNER_DEMAND);
        template.find('textarea[name=ADDITION]').val(value.ADDITION);

        var intervention = template.find('div[name=intervention]');
        $.each(value.INTERVENTION, function (index, value) {
            intervention.find('input[type=checkbox][value=' + value.PHRASE_KEY + ']').prop('checked', true);
        });
        template.find('textarea[name=INTERVENTION_MEMO]').val(value.INTERVENTION_MEMO);

        template.find('input[name=OPINION01][value=' + value.OPINION01 + ']').prop('checked', true);
        template.find('input[name=OPINION02][value=' + value.OPINION02 + ']').prop('checked', true);
        template.find('input[name=OPINION03][value=' + value.OPINION03 + ']').prop('checked', true);
        template.find('input[name=OPINION04][value=' + value.OPINION04 + ']').prop('checked', true);
        template.find('input[name=OPINION05][value=' + value.OPINION05 + ']').prop('checked', true);
        template.find('input[name=OPINION06][value=' + value.OPINION06 + ']').prop('checked', true);
        template.find('input[name=OPINION07][value=' + value.OPINION07 + ']').prop('checked', true);
        template.find('input[name=OPINION08][value=' + value.OPINION08 + ']').prop('checked', true);
        template.find('input[name=OPINION09][value=' + value.OPINION09 + ']').prop('checked', true);
        template.find('textarea[name=OPINION]').val(value.OPINION);
        template.find('input[name=CASE_STATUS][value=' + value.CASE_STATUS + ']').prop('checked', true);

        if (value.CDATE) {
            template.find('input[name=CDATE]').val(value.CDATE.replace('T', ' '));
        }
        template.find('input[name=CUSER]').val(value.CUSER);
        if (value.MDATE) {
            template.find('input[name=MDATE]').val(value.MDATE.replace('T', ' '));
        }
        template.find('input[name=MUSER]').val(value.MUSER);

        $('#detail_modal .modal-title').text('輔導記錄');
        $('#detail_modal .modal-body').html('');
        $('#detail_modal .modal-body').append(template);

        if (HistoryIndex.UserId === value.VOLUNTEER_SN && value.CASE_STATUS !== '02' && value.CASE_STATUS !== '04') {
            $('#detail_save').show();
            template.find('[name=CASE_STATUS][value="04"]').attr('disabled', true);
        }
        else {
            $('#detail_save').hide();
        }
        $('#detail_modal').modal('show');


    },

    DetailValidate: function () {
        var template = $('#detail_interview');
        var ErrMsg = '';
        var FocusObj = null;
        if (detail_interview) {
            if (!template.find('select[name=VOLUNTEER_SN]').val()) {
                ErrMsg += '此次接線者不可空白<br/>';
            }
            if (!template.find('input[name=INCOMING_DATE]').val()) {
                ErrMsg += '此次接線時間不可空白<br/>';
            }
            var bool = /^\d+$/.test(template.find('input[name=DURING]').val());
            console.log(bool);
            if (!bool) {
                ErrMsg += '談話時間輸入錯誤<br/>';
            }
            else {
                var during = parseInt(template.find('input[name=DURING]').val());
                if (during < 1 || during > 1440) {
                    ErrMsg += '談話時間必需介於1~1440<br/>';
                }
            }
            if (!template.find('input[name=NAME]').val()) {
                ErrMsg += '此次來電者姓名不可空白<br/>';
            }
            if (!template.find('input[name=TEL]').val()) {
                ErrMsg += '此次來電者電話不可空白<br/>';
            }
            if (!template.find('select[name=CONTACT_TIME]').val()) {
                ErrMsg += '聯絡時間不可空白<br/>';
            }
            if (!template.find('select[name=TREATMENT]').val()) {
                ErrMsg += '處遇方式不可空白<br/>';
            }
            if (!template.find('select[name=CASE_SOURCE]').val()) {
                ErrMsg += '個案來源不可空白<br/>';
            }
            if (!template.find('select[name=GENDER]').val()) {
                ErrMsg += '主述性別不可空白<br/>';
            }
            if (!template.find('select[name=AGE]').val()) {
                ErrMsg += '年齡層不可空白<br/>';
            }
            if (!template.find('select[name=EDUCATION]').val()) {
                ErrMsg += '學歷不可空白<br/>';
            }
            if (!template.find('select[name=CAREER]').val()) {
                ErrMsg += '職業不可空白<br/>';
            }
            if (!template.find('select[name=CITY]').val()) {
                ErrMsg += '居住地不可空白<br/>';
            }
            if (!template.find('select[name=MARRIAGE]').val()) {
                ErrMsg += '目前婚姻感情狀態不可空白<br/>';
            }
            if (!template.find('select[name=PHYSIOLOGY]').val()) {
                ErrMsg += '生理障礙不可空白<br/>';
            }
            if (!template.find('select[name=PSYCHOLOGY]').val()) {
                ErrMsg += '心理障礙不可空白<br/>';
            }
            if (!template.find('select[name=VISITED]').val()) {
                ErrMsg += '是否曾經來電不可空白<br/>';
            }

            var special_identity = template.find('div[name=special_identity]')
            var checkboxs1 = special_identity.find('input[name=SPECIAL_IDENTITY]:checked');
            if (checkboxs1.length < 1) {
                ErrMsg += '特殊身份別請至少勾選一個以上<br/>';
            }

            var interview_classify = template.find('div[name=interview_classify]')
            var checkboxs1 = interview_classify.find('input[name=INTERVIEW_CLASSIFY]:checked');
            if (!checkboxs1.filter('[value=U1]').is(':checked')) {
                if (checkboxs1.length < 2) {
                    ErrMsg += '來電者主述問題分類之圈選請至少勾選兩個以上<br/>';
                }
            }

            if (!template.find('select[name=SOLVE_DEGREE]').val()) {
                ErrMsg += '案主解決問題的程度不可空白<br/>';
            }

            var feeling = template.find('div[name=feeling]')
            if (feeling.find('input[name=FEELING]:checked').length < 1) {
                ErrMsg += '案主在此困擾的情緒請至少勾選一個以上<br/>';
            }

            var intervention = template.find('div[name=intervention]')
            if (intervention.find('input[name=INTERVENTION]:checked').length < 1) {
                ErrMsg += '接案過程介入方式請至少勾選一個以上<br/>';
            }

            if (!$(template.find('input[name=OPINION01]:checked')).val()) {
                ErrMsg += '真心關心當事人請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION02]:checked')).val()) {
                ErrMsg += '能接納當事人請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION03]:checked')).val()) {
                ErrMsg += '能引導當事人充分的敘說請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION04]:checked')).val()) {
                ErrMsg += '與當事人建立有助益的關係請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION05]:checked')).val()) {
                ErrMsg += '建立明確的協談目標請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION06]:checked')).val()) {
                ErrMsg += '充分了解當事人的問題請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION07]:checked')).val()) {
                ErrMsg += '純熟的協談技巧請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION08]:checked')).val()) {
                ErrMsg += '清晰的協談意圖請勾選適當的空格<br/>';
            }
            if (!$(template.find('input[name=OPINION09]:checked')).val()) {
                ErrMsg += '能訂定適當的輔導方向請勾選適當的空格<br/>';
            }

            if (ErrMsg) {
                //$('#detail_modal').animate({
                //    scrollTop: 0
                //}, 200);
                $('#modal .modal-title').text('欄位內容檢核');
                $('#modal .modal-body').html('<p>' + ErrMsg + '</p>');
                $('#modal').modal('show');
                return false;
            }
            else {
                return true;
            }
        }
    },

    DetailSave: function () {
        if (HistoryIndex.DetailValidate()) {
            var detail_section = $('#detail_interview');

            var SPECIAL_IDENTITY = [];
            var special_identity = detail_section.find('div[name=special_identity]');
            special_identity.find('input:checked').each(function () {
                SPECIAL_IDENTITY.push({
                    PHRASE_KEY: $(this).val(),
                    MEMO: special_identity.find('[name=SPECIAL_IDENTITY' + $(this).val() + ']').val()
                });
            });

            var INTERVIEW_CLASSIFY = [];
            var interview_classify = detail_section.find('div[name=interview_classify]');
            interview_classify.find('input:checked').each(function () {
                INTERVIEW_CLASSIFY.push({
                    PHRASE_KEY: $(this).val()
                });
            });

            var FEELING = [];
            var feeling = detail_section.find('div[name=feeling]');
            feeling.find('input:checked').each(function () {
                FEELING.push({
                    PHRASE_KEY: $(this).val()
                });
            });

            var INTERVENTION = [];
            var intervention = detail_section.find('div[name=intervention]');
            intervention.find('input:checked').each(function () {
                INTERVENTION.push({
                    PHRASE_KEY: $(this).val()
                });
            });

            var detail = {
                SN: detail_section.find('[name=SN]').val(),
                VOLUNTEER_SN: detail_section.find('[name=VOLUNTEER_SN]').val(),
                INCOMING_DATE: detail_section.find('[name=INCOMING_DATE]').val(),
                DURING: detail_section.find('[name=DURING]').val(),
                CASE_NO: detail_section.find('[name=CASE_NO]').val(),
                NAME: detail_section.find('[name=NAME]').val(),
                TEL: detail_section.find('[name=TEL]').val(),
                CONTACT_TIME: detail_section.find('[name=CONTACT_TIME]').val(),
                TREATMENT: detail_section.find('[name=TREATMENT]').val(),
                TREATMENT_MEMO: detail_section.find('[name=TREATMENT_MEMO]').val(),
                CASE_SOURCE: detail_section.find('[name=CASE_SOURCE]').val(),
                CASE_SOURCE_MEMO: detail_section.find('[name=CASE_SOURCE_MEMO]').val(),
                GENDER: detail_section.find('[name=GENDER]').val(),
                AGE: detail_section.find('[name=AGE]').val(),
                EDUCATION: detail_section.find('[name=EDUCATION]').val(),
                CAREER: detail_section.find('[name=CAREER]').val(),
                CAREER_MEMO: detail_section.find('[name=CAREER_MEMO]').val(),
                CITY: detail_section.find('[name=CITY]').val(),
                MARRIAGE: detail_section.find('[name=MARRIAGE]').val(),
                MARRIAGE_MEMO: detail_section.find('[name=MARRIAGE_MEMO]').val(),
                PHYSIOLOGY: detail_section.find('[name=PHYSIOLOGY]').val(),
                PHYSIOLOGY_MEMO: detail_section.find('[name=PHYSIOLOGY_MEMO]').val(),
                PSYCHOLOGY: detail_section.find('[name=PSYCHOLOGY]').val(),
                PSYCHOLOGY_MEMO: detail_section.find('[name=PSYCHOLOGY_MEMO]').val(),
                VISITED: detail_section.find('[name=VISITED]').val(),
                FAMILY: detail_section.find('[name=FAMILY]').val(),
                EXPERIENCE: detail_section.find('[name=EXPERIENCE]').val(),
                HARASS: detail_section.find('[name=HARASS]').val(),
                SOLVE_DEGREE: detail_section.find('[name=SOLVE_DEGREE]').val(),
                FEELING_MEMO: detail_section.find('[name=FEELING_MEMO]').val(),
                ABOUT_SELF: detail_section.find('[name=ABOUT_SELF]').val(),
                ABOUT_OTHERS: detail_section.find('[name=ABOUT_OTHERS]').val(),
                BEHAVIOR: detail_section.find('[name=BEHAVIOR]').val(),
                ADDITION: detail_section.find('[name=ADDITION]').val(),
                INNER_DEMAND: detail_section.find('[name=INNER_DEMAND]').val(),
                INTERVENTION_MEMO: detail_section.find('[name=INTERVENTION_MEMO]').val(),
                OPINION01: detail_section.find('input[name=OPINION01]:checked').val(),
                OPINION02: detail_section.find('input[name=OPINION02]:checked').val(),
                OPINION03: detail_section.find('input[name=OPINION03]:checked').val(),
                OPINION04: detail_section.find('input[name=OPINION04]:checked').val(),
                OPINION05: detail_section.find('input[name=OPINION05]:checked').val(),
                OPINION06: detail_section.find('input[name=OPINION06]:checked').val(),
                OPINION07: detail_section.find('input[name=OPINION07]:checked').val(),
                OPINION08: detail_section.find('input[name=OPINION08]:checked').val(),
                OPINION09: detail_section.find('input[name=OPINION09]:checked').val(),
                OPINION: detail_section.find('[name=OPINION]').val(),
                CASE_STATUS: detail_section.find('input[name=CASE_STATUS]:checked').val(),
                SPECIAL_IDENTITY: SPECIAL_IDENTITY,
                INTERVIEW_CLASSIFY: INTERVIEW_CLASSIFY,
                FEELING: FEELING,
                INTERVENTION: INTERVENTION,
                DataRow: detail_section.data('row')
            };

            if (detail_section.data('row') === 'add') {
                HistoryIndex.INTERVIEW.unshift(detail);
            }
            else if (detail_section.data('row') === 'edit') {
                HistoryIndex.INTERVIEW[detail_section.data('index')] = detail;
            }
            //console.log(HistoryIndex.INTERVIEW)

            $('#detail_modal').modal('hide');

            HistoryIndex.CaseUpdate();
            //HistoryIndex.DetailRefresh();
        }
    },

    ValueRecover: function (action) {
        var section_modify = $("#section_modify");
        if (action === 'U') {
            HistoryIndex.CaseQuery(section_modify.find('[name=SN]').val());
        }
        else {
            var master_sectoin = section_modify.find('#master_sectoin');
            master_sectoin.find('.fields').each(function () {
                $(this).val('');
            });
            master_sectoin.find('[name=FAMILY_FILE]').attr('src', '');

            HistoryIndex.INTERVIEW = [];
            HistoryIndex.DetailRefresh();
            //$('#detail_section').html('');
        }
    },

};
