var RolesIndex = {
    Action: null,
    ROLES: null,

    Page_Init: function () {
        this.EventBinding();
        this.OptionRetrieve();
        this.ActionSwitch('R');
        this.DataValidate();
    },

    EventBinding: function () {
        $('#query').click(function () {
            if ($('#section_retrieve').valid()) {
                RolesIndex.RolesRetrieve();
            }
        });
        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                RolesIndex.RolesRetrieve();
            }
        });
        
        $('#create').click(function () {
            RolesIndex.ActionSwitch('C');
        });

        $('#excel').click(function () {
            RolesIndex.RolesExcel();
        });

        $('#save').click(function () {
            if ($('#section_modify').valid()) {
                if (RolesIndex.Action === 'C') {
                    RolesIndex.RolesCreate();
                } else if (RolesIndex.Action === 'U') {
                    RolesIndex.RolesUpdate();
                }
            }
        });

        $('#undo').click(function () {
            RolesIndex.ValueRecover(RolesIndex.Action);
        });

        $('#delete').click(function () {
            $('#modal .modal-title').text('提示訊息');
            $('#modal .modal-body').html('<p>確定要刪除該筆資料?</p>');
            $('#modal #confirm').attr('data-action', 'delete');
            $('#modal #confirm').show();
            $('#modal').modal('show');
        });

        $('#return').click(function () {
            RolesIndex.ActionSwitch('R');
            RolesIndex.ValueRecover();
            if ($('#section_retrieve').valid()) {
                RolesIndex.RolesRetrieve();
            }
        });

        $('#modal #confirm').click(function () {
            $('#modal #confirm').hide();
            $('#modal').modal('hide');
            RolesIndex.RolesDelete();
        });
    },

    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#create').show();
            $('#excel').show();
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
            TableItem: ['userName'],
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

    RolesRetrieve: function () {
        var url = 'RolesRetrieve';
        var request = {
            ROLES: {
				SN: $('#section_retrieve #SN').val(),
				NAME: $('#section_retrieve #NAME').val(),
				MODE: $('#section_retrieve #MODE').val(),
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
                        $.each(response.ROLES, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="RolesIndex.RowSelected(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
							htmlRow += '<td>' + row.SN + '</td>';
							htmlRow += '<td>' + row.NAME + '</td>';
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

    RolesCreate: function () {
        var url = 'RolesCreate';
        var request = {
            ROLES: {
				SN: $('#section_modify #SN').val(),
				NAME: $('#section_modify #NAME').val(),
				MODE: $('#section_modify #MODE').val(),
				CDATE: $('#section_modify #CDATE').val(),
				CUSER: $('#section_modify #CUSER').val(),
				MDATE: $('#section_modify #MDATE').val(),
				MUSER: $('#section_modify #MUSER').val(),
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                console.log(data);
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 1) {
					$('#section_modify #SN').val(response.ROLES.SN);
					$('#section_modify #NAME').val(response.ROLES.NAME);
					$('#section_modify #MODE').val(response.ROLES.MODE);
                    $('#section_modify #CDATE').val(response.ROLES.CDATE.replace('T', ' '));
                    $('#section_modify #CUSER').val(response.ROLES.CUSER);
                    $('#section_modify #MDATE').val(response.ROLES.MDATE.replace('T', ' '));
                    $('#section_modify #MUSER').val(response.ROLES.MUSER);

                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易說明:' + response.ReturnStatus.Message + '<br /> 交易代碼:' + response.ReturnStatus.Code + '</p>');
                    $('#modal').modal('show');

                    RolesIndex.ActionSwitch('U');
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

    RolesUpdate: function () {
        var url = 'RolesUpdate';
        var request = {
            ROLES: {
				SN: $('#section_modify #SN').val(),
				NAME: $('#section_modify #NAME').val(),
				MODE: $('#section_modify #MODE').val(),
				CDATE: $('#section_modify #CDATE').val(),
				CUSER: $('#section_modify #CUSER').val(),
				MDATE: $('#section_modify #MDATE').val(),
				MUSER: $('#section_modify #MUSER').val(),
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                console.log(data);
                if (response.ReturnStatus.Code === 2) {
					$('#section_modify #SN').val(response.ROLES.SN);
					$('#section_modify #NAME').val(response.ROLES.NAME);
					$('#section_modify #MODE').val(response.ROLES.MODE);
                    $('#section_modify #CDATE').val(response.ROLES.CDATE.replace('T', ' '));
                    $('#section_modify #CUSER').val(response.ROLES.CUSER);
                    $('#section_modify #MDATE').val(response.ROLES.MDATE.replace('T', ' '));
                    $('#section_modify #MUSER').val(response.ROLES.MUSER);

                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易說明:' + response.ReturnStatus.Message + '<br /> 交易代碼:' + response.ReturnStatus.Code + '</p>');
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

    RolesDelete: function () {
        var url = 'RolesDelete';
        var request = {
            ROLES: {
				SN: $('#section_modify #SN').val(),
				NAME: $('#section_modify #NAME').val(),
				MODE: $('#section_modify #MODE').val(),
				CDATE: $('#section_modify #CDATE').val(),
				CUSER: $('#section_modify #CUSER').val(),
				MDATE: $('#section_modify #MDATE').val(),
				MUSER: $('#section_modify #MUSER').val(),                
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
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易說明:' + response.ReturnStatus.Message + '<br /> 交易代碼:' + response.ReturnStatus.Code + '</p>');
                    $('#modal').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#modal .modal-title').text(ajaxOptions);
                $('#modal .modal-body').html('<p>' + xhr.status + ' ' + thrownError + '</p>');
                $('#modal').modal('show');
            },
            complete: function (xhr, status) {
                if ($('#section_retrieve').valid()) {
                    RolesIndex.RolesRetrieve();
                    RolesIndex.ActionSwitch('R');
                }
            }
        });
    },

    RolesQuery: function (SN) {
        var url = 'RolesQuery';
        var request = {
            ROLES: {
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
					$('#section_modify #SN').val(response.ROLES.SN);
					$('#section_modify #NAME').val(response.ROLES.NAME);
					$('#section_modify #MODE').val(response.ROLES.MODE);
                    $('#section_modify #CDATE').val(response.ROLES.CDATE.replace('T', ' '));
                    $('#section_modify #CUSER').val(response.ROLES.CUSER);
                    $('#section_modify #MDATE').val(response.ROLES.MDATE.replace('T', ' '));
                    $('#section_modify #MUSER').val(response.ROLES.MUSER);

                    RolesIndex.ROLES = response.ROLES;
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

    RolesExcel: function () {
        var url = 'RolesExcel';
        var request = {
            ROLES: {
				SN: $('#section_retrieve #SN').val(),
				NAME: $('#section_retrieve #NAME').val(),
				MODE: $('#section_retrieve #MODE').val(),
				CDATE: $('#section_retrieve #CDATE').val(),
				CUSER: $('#section_retrieve #CUSER').val(),
				MDATE: $('#section_retrieve #MDATE').val(),
				MUSER: $('#section_retrieve #MUSER').val(),
            },
        };
        window.location.href = url + '?json=' + JSON.stringify(request);
    },

    RowSelected: function (key) {
        RolesIndex.RolesQuery(key);
        this.ActionSwitch('U');
    },

    DataValidate: function () {
        $('#section_modify').validate({
            rules: {
				SN: 'required',
				NAME: 'required',
				MODE: 'required',
				CDATE: 'required',
				CUSER: 'required',
				MDATE: 'required',
				MUSER: 'required',
            }
        });
    },

    ValueRecover: function (action) {
        if (action == 'U') {
            if (RolesIndex.ROLES) {
                $('.modify').each(function (index, value) {
                    $(value).val(RolesIndex.ROLES[value.id]);
                });
            }
        }
        else {
            $('.modify').each(function (index, value) {
                $(value).val('');
            });
        }
    },

};
