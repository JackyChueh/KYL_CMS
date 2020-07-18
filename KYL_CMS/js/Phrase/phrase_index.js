var PhraseIndex = {
    Action: null,
    validator: null,
    PHRASE: null,

    Page_Init: function () {
        this.EventBinding();
        this.OptionRetrieve();
        this.ActionSwitch('R');
        this.DataValidate();
    },

    EventBinding: function () {
        $('#query').click(function () {
            if ($('#section_retrieve').valid()) {
                PhraseIndex.PhraseRetrieve();
            }
        });
        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                PhraseIndex.PhraseRetrieve();
            }
        });
        
        $('#create').click(function () {
            $('#section_modify #PHRASE_GROUP').val($('#section_retrieve #PHRASE_GROUP').val())
            PhraseIndex.ActionSwitch('C');
        });

      
        $('#save').click(function () {
            if ($('#section_modify').valid()) {
                if (PhraseIndex.Action === 'C') {
                    PhraseIndex.PhraseCreate();
                } else if (PhraseIndex.Action === 'U') {
                    PhraseIndex.PhraseUpdate();
                }
            }
        });

        $('#undo').click(function () {
            PhraseIndex.ValueRecover(PhraseIndex.Action);
        });

        $('#delete').click(function () {
            $('#modal .modal-title').text('提示訊息');
            $('#modal .modal-body').html('<p>確定要刪除該筆資料?</p>');
            $('#modal #confirm').attr('data-action', 'delete');
            $('#modal #confirm').show();
            $('#modal').modal('show');
        });

        $('#return').click(function () {
            PhraseIndex.ActionSwitch('R');
            PhraseIndex.ValueRecover();
            if ($('#section_retrieve').valid()) {
                PhraseIndex.PhraseRetrieve();
            }
        });

        $('#modal #confirm').click(function () {
            $('#modal #confirm').hide();
            $('#modal').modal('hide');
            PhraseIndex.PhraseDelete();
        });
    },

    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#create').show();
            $('#section_retrieve').show();
        } else if (action === 'U') {
            $('#save').show();
            $('#delete').show();
            $('#return').show();
            $('#undo').show();
            $('#section_modify').show();
        } else if (action === 'C') {
            $('#save').show();
            $('#return').show();
            $('#undo').show();
            $('#section_modify').show();
        }
        this.Action = action;
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            PhraseGroup: ['mode', 'page_size']
        };

        $.ajax({
            async: false,
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);

                if (response.ReturnStatus.Code === 0) {
                    $('form #MODE').append('<option value=""></option>');
                    $.each(response.ItemList.mode, function (idx, row) {
                        $('form #MODE').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    $.each(response.ItemList.page_size, function (idx, row) {
                        $('#page_size').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    var phrase_group = $('form #PHRASE_GROUP');
                    phrase_group.append($('<option></option>').attr('value', 'CONTACT_TIME').text('聯絡時間'));
                    phrase_group.append($('<option></option>').attr('value', 'TREATMENT').text('處遇方式'));
                    phrase_group.append($('<option></option>').attr('value', 'CASE_SOURCE').text('個案來源'));
                    phrase_group.append($('<option></option>').attr('value', 'GENDER').text('主述性別'));
                    phrase_group.append($('<option></option>').attr('value', 'AGE').text('年齡層'));
                    phrase_group.append($('<option></option>').attr('value', 'EDUCATION').text('學歷'));
                    phrase_group.append($('<option></option>').attr('value', 'CAREER').text('職業'));
                    phrase_group.append($('<option></option>').attr('value', 'CITY').text('居住地'));
                    phrase_group.append($('<option></option>').attr('value', 'MARRIAGE').text('目前婚姻感情狀態'));
                    phrase_group.append($('<option></option>').attr('value', 'PHYSIOLOGY').text('生理障礙'));
                    phrase_group.append($('<option></option>').attr('value', 'PSYCHOLOGY').text('心理障礙'));
                    phrase_group.append($('<option></option>').attr('value', 'VISITED').text('是否曾經來電'));
                    phrase_group.append($('<option></option>').attr('value', 'SPECIAL_IDENTITY').text('特殊身份別'));
                    phrase_group.append($('<option></option>').attr('value', 'INTERVIEW_CLASSIFY').text('主述問題分類'));
                    phrase_group.append($('<option></option>').attr('value', 'SOLVE_DEGREE').text('案主解決問題的程度'));
                    phrase_group.append($('<option></option>').attr('value', 'FEELING').text('案主在此困擾的情緒'));
                    phrase_group.append($('<option></option>').attr('value', 'INTERVENTION').text('接案過程介入方式'));
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

    PhraseRetrieve: function () {
        var url = 'PhraseRetrieve';
        var request = {
            PHRASE: {
				PHRASE_GROUP: $('#section_retrieve #PHRASE_GROUP').val(),
				PHRASE_KEY: $('#section_retrieve #PHRASE_KEY').val(),
				PHRASE_VALUE: $('#section_retrieve #PHRASE_VALUE').val(),
				MODE: $('#section_retrieve #MODE').val()
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
                        $.each(response.PHRASE, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="PhraseIndex.RowSelected(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
							htmlRow += '<td>' + row.SN + '</td>';
							htmlRow += '<td>' + row.PHRASE_KEY + '</td>';
							htmlRow += '<td>' + row.PHRASE_VALUE + '</td>';
                            htmlRow += '<td>' + (row.PHRASE_DESC ? row.PHRASE_DESC : '')  + '</td>';
							htmlRow += '<td>' + row.SORT + '</td>';
							htmlRow += '<td>' + row.MODE + '</td>';
                            htmlRow += '<td>' + row.CDATE.replace('T', ' ') + '</td>';
							htmlRow += '<td>' + row.CUSER + '</td>';
                            htmlRow += '<td>' + row.MDATE.replace('T', ' ') + '</td>';
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

    PhraseCreate: function () {
        var url = 'PhraseCreate';
        var request = {
            PHRASE: {
				SN: $('#section_modify #SN').val(),
				PHRASE_GROUP: $('#section_modify #PHRASE_GROUP').val(),
				PHRASE_KEY: $('#section_modify #PHRASE_KEY').val(),
				PHRASE_VALUE: $('#section_modify #PHRASE_VALUE').val(),
				PHRASE_DESC: $('#section_modify #PHRASE_DESC').val(),
				SORT: $('#section_modify #SORT').val(),
				MODE: $('#section_modify #MODE').val()
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                console.log(data)
                if (response.ReturnStatus.Code === 1) {
                    //console.log(response)
					//$('#section_modify #SN').val(response.PHRASE.SN);
					//$('#section_modify #PHRASE_GROUP').val(response.PHRASE.PHRASE_GROUP);
					//$('#section_modify #PHRASE_KEY').val(response.PHRASE.PHRASE_KEY);
					//$('#section_modify #PHRASE_VALUE').val(response.PHRASE.PHRASE_VALUE);
					//$('#section_modify #PHRASE_DESC').val(response.PHRASE.PHRASE_DESC);
					//$('#section_modify #SORT').val(response.PHRASE.SORT);
					//$('#section_modify #MODE').val(response.PHRASE.MODE);
     //               $('#section_modify #CDATE').val(response.PHRASE.CDATE.replace('T', ' '));
					//$('#section_modify #CUSER').val(response.PHRASE.CUSER);
     //               $('#section_modify #MDATE').val(response.PHRASE.MDATE.replace('T', ' '));
					//$('#section_modify #MUSER').val(response.PHRASE.MUSER);
                    PhraseIndex.PhraseQuery(response.PHRASE.SN);
                }
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>交易說明:' + response.ReturnStatus.Message + '<br /> 交易代碼:' + response.ReturnStatus.Code + '</p>');
                $('#modal').modal('show');
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

    PhraseUpdate: function () {
        var url = 'PhraseUpdate';
        var request = {
            PHRASE: {
				SN: $('#section_modify #SN').val(),
				PHRASE_GROUP: $('#section_modify #PHRASE_GROUP').val(),
				PHRASE_KEY: $('#section_modify #PHRASE_KEY').val(),
				PHRASE_VALUE: $('#section_modify #PHRASE_VALUE').val(),
				PHRASE_DESC: $('#section_modify #PHRASE_DESC').val(),
                SORT: $('#section_modify #SORT').val(),
                MODE: $('#section_modify #MODE').val()
            }
        };
        console.log(request)
        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 2) {
					//$('#section_modify #SN').val(response.PHRASE.SN);
					//$('#section_modify #PHRASE_GROUP').val(response.PHRASE.PHRASE_GROUP);
					//$('#section_modify #PHRASE_KEY').val(response.PHRASE.PHRASE_KEY);
					//$('#section_modify #PHRASE_VALUE').val(response.PHRASE.PHRASE_VALUE);
					//$('#section_modify #PHRASE_DESC').val(response.PHRASE.PHRASE_DESC);
					//$('#section_modify #SORT').val(response.PHRASE.SORT);
					//$('#section_modify #MODE').val(response.PHRASE.MODE);
     //               $('#section_modify #CDATE').val(response.PHRASE.CDATE.replace('T', ' '));
					//$('#section_modify #CUSER').val(response.PHRASE.CUSER);
     //               $('#section_modify #MDATE').val(response.PHRASE.MDATE.replace('T', ' '));
     //               $('#section_modify #MUSER').val(response.PHRASE.MUSER);
                    PhraseIndex.PhraseQuery(response.PHRASE.SN);
                }
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>交易說明:' + response.ReturnStatus.Message + '<br /> 交易代碼:' + response.ReturnStatus.Code + '</p>');
                $('#modal').modal('show');
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

    PhraseDelete: function () {
        var url = 'PhraseDelete';
        var request = {
            PHRASE: {
                SN: $('#section_modify #SN').val(),
                PHRASE_GROUP: $('#section_modify #PHRASE_GROUP').val(),
                PHRASE_KEY: $('#section_modify #PHRASE_KEY').val()
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 3) {
                    if ($('#section_retrieve').valid()) {
                        PhraseIndex.PhraseRetrieve();
                        PhraseIndex.ActionSwitch('R');
                        PhraseIndex.ValueRecover();
                    }
                }
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>交易說明:' + response.ReturnStatus.Message + '<br /> 交易代碼:' + response.ReturnStatus.Code + '</p>');
                $('#modal').modal('show');
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

    PhraseQuery: function (SN) {
        var url = 'PhraseQuery';
        var request = {
            PHRASE: {
                SN: SN
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 0) {
					$('#section_modify #SN').val(response.PHRASE.SN);
					$('#section_modify #PHRASE_GROUP').val(response.PHRASE.PHRASE_GROUP);
					$('#section_modify #PHRASE_KEY').val(response.PHRASE.PHRASE_KEY);
					$('#section_modify #PHRASE_VALUE').val(response.PHRASE.PHRASE_VALUE);
					$('#section_modify #PHRASE_DESC').val(response.PHRASE.PHRASE_DESC);
					$('#section_modify #SORT').val(response.PHRASE.SORT);
					$('#section_modify #MODE').val(response.PHRASE.MODE);
                    $('#section_modify #CDATE').val(response.PHRASE.CDATE.replace('T', ' '));
					$('#section_modify #CUSER').val(response.PHRASE.CUSER);
                    $('#section_modify #MDATE').val(response.PHRASE.MDATE.replace('T', ' '));
					$('#section_modify #MUSER').val(response.PHRASE.MUSER);
                    PhraseIndex.PHRASE = response.PHRASE;
                    PhraseIndex.ActionSwitch('U');
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
        PhraseIndex.PhraseQuery(key);
        this.ActionSwitch('U');
    },

    DataValidate: function () {
        PhraseIndex.validator = $('#section_modify').validate({
            rules: {
				SN: 'required',
				PHRASE_GROUP: 'required',
				PHRASE_KEY: 'required',
                PHRASE_VALUE: 'required',
                MODE: 'required',
                SORT: {
                    required: true,
                    digits: true
                }
            }
        });
    },

    ValueRecover: function (action) {
        PhraseIndex.validator.resetForm();
        if (action == 'U') {
            var section_modify = $("#section_modify")
            PhraseIndex.PhraseQuery(section_modify.find('[name=SN]').val());
            //if (PhraseIndex.PHRASE) {
            //    $('.modify').each(function (index, value) {
            //        $(value).val(PhraseIndex.PHRASE[value.id]);
            //    });
            //}
        }
        else {
            $('.modify').each(function (index, value) {
                $(value).val('');
            });
        }
    },

};
