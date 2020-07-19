var GrantsIndex = {
    Action: null,
    GRANTS: null,

    Page_Init: function () {
        this.EventBinding();
        //this.OptionRetrieve();
        //this.ActionSwitch('R');
        this.DataValidate();
        $("#users").trigger("click")
    },

    EventBinding: function () {

        //觸發設定方式
        $('input[name=mode]').change(function () {
            GrantsIndex.ActionSwitch(this.id);
            $('#action').html(this.value);
        });

        //觸發選擇帳號或群組
        $('select[name=users_roles]').change(function () {
            if ($(this).val() === '') {
                var joined = $('#joined');
                joined.html('');
                var unjoin = $('#unjoin');
                unjoin.html('');
            } else {
                if (GrantsIndex.Action === 'users') {
                    GrantsIndex.UsersGrantsQuery($(this).val())
                } else if(GrantsIndex.Action === 'roles') {
                    GrantsIndex.RolesGrantsQuery($(this).val())
                }
            }
            
        });

        //加入&移除
        $('#add').click(function () {
            var options = $('#unjoin option:selected');

            if (GrantsIndex.Action === 'users' && $('#joined option').length + options.length > 1) {
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>只能加入一個群組</p>');
                $('#modal').modal('show');
                return false;
            }
            
            options.remove();
            $("#joined").append(options);
        });
        $('#remove').click(function () {
            var options = $('#joined option:selected');
            options.remove();
            $("#unjoin").append(options);
        });

        //儲存
        $('#save').click(function () {
            if ($('#section_modify').valid()) {
                if (GrantsIndex.Action === 'users') {
                    GrantsIndex.UsersGrantsUpdate();
                } else if (GrantsIndex.Action === 'roles') {
                    GrantsIndex.RolersGrantsUpdate();
                }
                
            }
        });

        //復原
        $('#undo').click(function () {
            GrantsIndex.ValueRecover(GrantsIndex.Action);
        });

    },

    ActionSwitch: function (action) {
        if (action === 'users') {
            this.UsersOptionRetrieve();
            var joined = $('#joined').html('');
            var unjoin = $('#unjoin').html('');
        } else if (action === 'roles') {
            this.RolesOptionRetrieve();
            var joined = $('#joined').html('');
            var unjoin = $('#unjoin').html('');
        }
        this.Action = action;
    },

    UsersOptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            TableItem: ['UserIdName'],
            PhraseGroup: []
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

                    obj = $('select[name=users_roles]');
                    obj.html('');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.UserIdName, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });


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
                //alert('' + xhr.status + ';' + status );
            }
        });
    },
    RolesOptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            TableItem: ['RolesName'],
            PhraseGroup: []
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

                    obj = $('select[name=users_roles]');
                    obj.html('');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.RolesName, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });


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
                //alert('' + xhr.status + ';' + status );
            }
        });
    },

    GrantsRetrieve: function () {
        var url = 'GrantsRetrieve';
        var request = {
            GRANTS: {
				ROLES_SN: $('#section_retrieve #ROLES_SN').val(),
				USERS_SN: $('#section_retrieve #USERS_SN').val(),
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
                        $.each(response.GRANTS, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="GrantsIndex.RowSelected(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
							htmlRow += '<td>' + row.ROLES_SN + '</td>';
							htmlRow += '<td>' + row.USERS_SN + '</td>';
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

    GrantsCreate: function () {
        var url = 'GrantsCreate';
        var request = {
            GRANTS: {
				ROLES_SN: $('#section_modify #ROLES_SN').val(),
				USERS_SN: $('#section_modify #USERS_SN').val(),
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
					$('#section_modify #ROLES_SN').val(response.GRANTS.ROLES_SN);
					$('#section_modify #USERS_SN').val(response.GRANTS.USERS_SN);
					$('#section_modify #CDATE').val(response.GRANTS.CDATE);
					$('#section_modify #CUSER').val(response.GRANTS.CUSER);
					$('#section_modify #MDATE').val(response.GRANTS.MDATE);
					$('#section_modify #MUSER').val(response.GRANTS.MUSER);

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

    UsersGrantsUpdate: function () {
        var url = 'UsersGrantsUpdate';
        var request = {
            USERS_SN: [],
            ROLES_SN:[]
        };
        var users = []
        users.push($('select[name=users_roles]').val());
        request.USERS_SN = users;

        var roles = [];
        $("#joined option").each(function () {
            roles.push($(this).val());
        });
        request.ROLES_SN = roles;
        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                //console.log(data)
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 4) {
                    //
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

    RolersGrantsUpdate: function () {
        var url = 'RolersGrantsUpdate';
        var request = {
            USERS_SN: [],
            ROLES_SN: []
        };

        var roles = [];
        roles.push($('select[name=users_roles]').val());
        request.ROLES_SN = roles;

        var users = []
        $("#joined option").each(function () {
            users.push($(this).val());
        });
        request.USERS_SN = users;

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 4) {
                  //
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

    GrantsDelete: function () {
        var url = 'GrantsDelete';
        var request = {
            GRANTS: {
				ROLES_SN: $('#section_modify #ROLES_SN').val(),
				USERS_SN: $('#section_modify #USERS_SN').val(),
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
                    HostIndex.HostRetrieve();
                    HostIndex.ActionSwitch('R');
                }
            }
        });
    },

    UsersGrantsQuery: function (USERS_SN) {
        var url = 'UsersGrantsQuery';
        var request = {
            GRANTS: {
                USERS_SN: USERS_SN
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                //console.log(data)
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 0) {
                    var joined = $('#joined');
                    joined.html('');
                    var unjoin = $('#unjoin');
                    unjoin.html('');
                    $.each(response.GRANTS, function (idx, row) {
                        if (row.USERS_SN) {
                            joined.append($('<option></option>').attr('value', row.ROLES_SN).text(row.ROLES_NAME));
                        } else {
                            unjoin.append($('<option></option>').attr('value', row.ROLES_SN).text(row.ROLES_NAME));
                        }
                    });
					//$('#section_modify #ROLES_SN').val(response.GRANTS.ROLES_SN);
					//$('#section_modify #USERS_SN').val(response.GRANTS.USERS_SN);
					//$('#section_modify #CDATE').val(response.GRANTS.CDATE);
					//$('#section_modify #CUSER').val(response.GRANTS.CUSER);
					//$('#section_modify #MDATE').val(response.GRANTS.MDATE);
					//$('#section_modify #MUSER').val(response.GRANTS.MUSER);

                    //HostIndex.GRANTS = response.GRANTS;
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

    RolesGrantsQuery: function (ROLES_SN) {
        var url = 'RolesGrantsQuery';
        var request = {
            GRANTS: {
                ROLES_SN: ROLES_SN
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                //console.log(data)
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 0) {
                    var joined = $('#joined');
                    joined.html('');
                    var unjoin = $('#unjoin');
                    unjoin.html('');
                    $.each(response.GRANTS, function (idx, row) {
                        if (row.ROLES_SN) {
                            joined.append($('<option></option>').attr('value', row.USERS_SN).text(row.USERS_NAME));
                        } else {
                            unjoin.append($('<option></option>').attr('value', row.USERS_SN).text(row.USERS_NAME));
                        }
                    });
                    //$('#section_modify #ROLES_SN').val(response.GRANTS.ROLES_SN);
                    //$('#section_modify #USERS_SN').val(response.GRANTS.USERS_SN);
                    //$('#section_modify #CDATE').val(response.GRANTS.CDATE);
                    //$('#section_modify #CUSER').val(response.GRANTS.CUSER);
                    //$('#section_modify #MDATE').val(response.GRANTS.MDATE);
                    //$('#section_modify #MUSER').val(response.GRANTS.MUSER);

                    //HostIndex.GRANTS = response.GRANTS;
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

    GrantsExcel: function () {
        var url = 'GrantsExcel';
        var request = {
            GRANTS: {
				ROLES_SN: $('#section_retrieve #ROLES_SN').val(),
				USERS_SN: $('#section_retrieve #USERS_SN').val(),
				CDATE: $('#section_retrieve #CDATE').val(),
				CUSER: $('#section_retrieve #CUSER').val(),
				MDATE: $('#section_retrieve #MDATE').val(),
				MUSER: $('#section_retrieve #MUSER').val(),
            },
        };
        window.location.href = url + '?json=' + JSON.stringify(request);
    },

    RowSelected: function (key) {
        GrantsIndex.GrantsQuery(key);
        this.ActionSwitch('U');
    },

    DataValidate: function () {
        $('#section_modify').validate({
            rules: {
                users_roles : 'required'
            }
        });
    },

    ValueRecover: function (action) {
        $('select[name=users_roles]').change();    
    },

};
