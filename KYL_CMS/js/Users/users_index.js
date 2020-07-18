var UsersIndex = {
    Action: null,
    validator :null,
    USERS: null,
    ConfirmAction: null,

    Page_Init: function () {
        this.EventBinding();
        this.OptionRetrieve();
        this.ActionSwitch('R');
        this.DataValidate();
    },

    EventBinding: function () {
        $('#query').click(function () {
            if ($('#section_retrieve').valid()) {
                UsersIndex.UsersRetrieve();
            }
        });
        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                UsersIndex.UsersRetrieve();
            }
        });
        
        $('#create').click(function () {
            UsersIndex.ActionSwitch('C');
        });

        $('#save').click(function () {
            if ($('#section_modify').valid()) {
                if (UsersIndex.Action === 'C') {
                    UsersIndex.UsersCreate();
                } else if (UsersIndex.Action === 'U') {
                    UsersIndex.UsersUpdate();
                }
            }
        });

        $('#undo').click(function () {
            UsersIndex.ValueRecover(UsersIndex.Action);
        });

        $('#delete').click(function () {
            $('#modal_action .modal-title').text('提示訊息');
            $('#modal_action .modal-body').html('<p>確定要刪除該筆資料?</p>');
            //$('#confirm').attr('data-action', 'delete');
            UsersIndex.ConfirmAction = 'delete';
            $('#modal_action #confirm').show();
            $('#modal_action').modal('show');
        });
        $('#reset').click(function () {
            $('#modal_action .modal-title').text('提示訊息');
            $('#modal_action .modal-body').html('<p>確定要重置密碼?</p>');
            //$('#confirm').attr('data-action', 'reset');
            UsersIndex.ConfirmAction = 'reset';
            $('#modal_action #confirm').show();
            $('#modal_action').modal('show');
        });


        $('#return').click(function () {
            UsersIndex.ActionSwitch('R');
            UsersIndex.ValueRecover();
            if ($('#section_retrieve').valid()) {
                UsersIndex.UsersRetrieve();
            }
        });

        $('#modal_action #confirm').click(function () {
            $('#confirm').hide();
            $('#modal_action').modal('hide');
            console.log(UsersIndex.ConfirmAction );
            if (UsersIndex.ConfirmAction  === 'delete') { 
                UsersIndex.UsersDelete();
            } if (UsersIndex.ConfirmAction === 'reset') {
                UsersIndex.UsersReset();
            }
        });

        $('#section_modify #ID').keyup(function () {
            $("#section_modify #ID").val(($("#section_modify #ID").val()).toUpperCase());
        });
        $('#section_modify #PASSWORD').keyup(function () {
            $("#section_modify #PASSWORD").val(($("#section_modify #PASSWORD").val()).toUpperCase());
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
            $('#reset').show();
            $('#section_modify #ID').prop('disabled', true);
            $('#section_modify').show();
        } else if (action === 'C') {
            $('#save').show();
            $('#return').show();
            $('#undo').show();
            $('#section_modify #ID').prop('disabled', false);
            $('#section_modify').show();
        }
        this.Action = action;
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            PhraseGroup: ['mode','page_size']
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

    UsersRetrieve: function () {
        var url = 'UsersRetrieve';
        var request = {
            USERS: {
				SN: $('#section_retrieve #SN').val(),
				ID: $('#section_retrieve #ID').val(),
				NAME: $('#section_retrieve #NAME').val(),
				PASSWORD: $('#section_retrieve #PASSWORD').val(),
				EMAIL: $('#section_retrieve #EMAIL').val(),
				MODE: $('#section_retrieve #MODE').val(),
				MEMO: $('#section_retrieve #MEMO').val(),
				CDATE: $('#section_retrieve #CDATE').val(),
				CUSER: $('#section_retrieve #CUSER').val(),
				MDATE: $('#section_retrieve #MDATE').val(),
				MUSER: $('#section_retrieve #MUSER').val()
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
                        $.each(response.USERS, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="UsersIndex.RowSelected(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
                            htmlRow += '<td>' + row.SN + '</td>';
                            htmlRow += '<td>' + row.ID + '</td>';
                            htmlRow += '<td>' + row.NAME + '</td>';
                            htmlRow += '<td>' + (row.EMAIL ? row.EMAIL : '') + '</td>';
                            htmlRow += '<td>' + row.MODE + '</td>';
                            htmlRow += '<td>' + (row.MEMO ? row.MEMO : '') + '</td>';
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

    UsersCreate: function () {
        var url = 'UsersCreate';
        var request = {
            USERS: {
				ID: $('#section_modify #ID').val(),
				NAME: $('#section_modify #NAME').val(),
				PASSWORD: $('#section_modify #PASSWORD').val(),
				EMAIL: $('#section_modify #EMAIL').val(),
				MODE: $('#section_modify #MODE').val(),
				MEMO: $('#section_modify #MEMO').val()
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 1) {
					//$('#section_modify #SN').val(response.USERS.SN);
					//$('#section_modify #ID').val(response.USERS.ID);
					//$('#section_modify #NAME').val(response.USERS.NAME);
					//$('#section_modify #PASSWORD').val(response.USERS.PASSWORD);
					//$('#section_modify #EMAIL').val(response.USERS.EMAIL);
					//$('#section_modify #MODE').val(response.USERS.MODE);
					//$('#section_modify #MEMO').val(response.USERS.MEMO);
     //               $('#section_modify #CDATE').val(response.USERS.CDATE.replace('T', ' '));
					//$('#section_modify #CUSER').val(response.USERS.CUSER);
     //               $('#section_modify #MDATE').val(response.USERS.MDATE.replace('T', ' '));
                    //$('#section_modify #MUSER').val(response.USERS.MUSER);
                    UsersIndex.UsersQuery(response.USERS.SN);
                }
                UsersIndex.ActionSwitch('U');

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

    UsersUpdate: function () {
        var url = 'UsersUpdate';
        var request = {
            USERS: {
				SN: $('#section_modify #SN').val(),
				ID: $('#section_modify #ID').val(),
				NAME: $('#section_modify #NAME').val(),
				PASSWORD: $('#section_modify #PASSWORD').val(),
				EMAIL: $('#section_modify #EMAIL').val(),
				MODE: $('#section_modify #MODE').val(),
				MEMO: $('#section_modify #MEMO').val()
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 2) {
					//$('#section_modify #SN').val(response.USERS.SN);
					//$('#section_modify #ID').val(response.USERS.ID);
					//$('#section_modify #NAME').val(response.USERS.NAME);
					//$('#section_modify #PASSWORD').val(response.USERS.PASSWORD);
					//$('#section_modify #EMAIL').val(response.USERS.EMAIL);
					//$('#section_modify #MODE').val(response.USERS.MODE);
					//$('#section_modify #MEMO').val(response.USERS.MEMO);
     //               $('#section_modify #CDATE').val(response.USERS.CDATE.replace('T', ' '));
					//$('#section_modify #CUSER').val(response.USERS.CUSER);
     //               $('#section_modify #MDATE').val(response.USERS.MDATE.replace('T', ' '));
					//$('#section_modify #MUSER').val(response.USERS.MUSER);
                    UsersIndex.UsersQuery(response.USERS.SN);
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

    UsersDelete: function () {
        var url = 'UsersDelete';
        var request = {
            USERS: {
				SN: $('#section_modify #SN').val(),
				ID: $('#section_modify #ID').val(),
				NAME: $('#section_modify #NAME').val(),
				PASSWORD: $('#section_modify #PASSWORD').val(),
				EMAIL: $('#section_modify #EMAIL').val(),
				MODE: $('#section_modify #MODE').val(),
				MEMO: $('#section_modify #MEMO').val(),
				CDATE: $('#section_modify #CDATE').val(),
				CUSER: $('#section_modify #CUSER').val(),
				MDATE: $('#section_modify #MDATE').val(),
				MUSER: $('#section_modify #MUSER').val()
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
                    UsersIndex.UsersRetrieve();
                    UsersIndex.ActionSwitch('R');
                    UsersIndex.ValueRecover();
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

    UsersReset: function () {
        var url = 'UsersReset';
        var request = {
            USERS: {
                SN: $('#section_modify #SN').val(),
                ID: $('#section_modify #ID').val()
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
                    UsersIndex.UsersQuery(response.USERS.SN);
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

    UsersQuery: function (SN) {
        var url = 'UsersQuery';
        var request = {
            USERS: {
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
					$('#section_modify #SN').val(response.USERS.SN);
					$('#section_modify #ID').val(response.USERS.ID);
					$('#section_modify #NAME').val(response.USERS.NAME);
					$('#section_modify #EMAIL').val(response.USERS.EMAIL);
					$('#section_modify #MODE').val(response.USERS.MODE);
					$('#section_modify #MEMO').val(response.USERS.MEMO);
                    $('#section_modify #CDATE').val(response.USERS.CDATE.replace('T', ' '));
					$('#section_modify #CUSER').val(response.USERS.CUSER);
                    $('#section_modify #MDATE').val(response.USERS.MDATE.replace('T', ' '));
					$('#section_modify #MUSER').val(response.USERS.MUSER);
                    UsersIndex.USERS = response.USERS;
                    UsersIndex.ActionSwitch('U');
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
        UsersIndex.UsersQuery(key);
        this.ActionSwitch('U');
    },

    DataValidate: function () {
        UsersIndex.validator = $('#section_modify').validate({
            rules: {
                ID: 'required',
                NAME: 'required',
                EMAIL: {
                    required: false,
                    email: true
                },
                MODE: 'required'
            }
        });
    },

    ValueRecover: function (action) {
        UsersIndex.validator.resetForm();
        if (action === 'U') {
            if (UsersIndex.USERS) {
                $('.modify').each(function (index, value) {
                    $(value).val(UsersIndex.USERS[value.id]);
                });
            }
        }
        else {
            $('.modify').each(function (index, value) {
                $(value).val('');
            });
        }
    }

};
