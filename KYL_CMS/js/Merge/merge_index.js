var MergeIndex = {
    Action: null,
    CASE: [],
    MERGE: [],

    Page_Init: function () {
        MergeIndex.EventBinding();
        MergeIndex.OptionRetrieve();
        MergeIndex.DataValidate();
    },

    EventBinding: function () {
        $('#query').click(function () {
            if ($('#section_retrieve').valid()) {
                MergeIndex.CaseRetrieve();
            }
        });

        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                MergeIndex.CaseRetrieve();
            }
        });

        $('button[name=undo]').click(function () {
            var SN = $(this).val();
            var SelectedIndex = null;
            $.each(MergeIndex.MERGE, function (index, value) {
                if (SN == value.SN) {
                    SelectedIndex = index;
                }
            });
            MergeIndex.MERGE.splice(SelectedIndex, 1);
            MergeIndex.SetData();
        });

        $('#save').click(function () {
            if ($('#section_modify').valid()) {
                MergeIndex.CaseMerge()
            }
        });
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            TableItem: [],
            PhraseGroup: ['page_size']
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

    CaseRetrieve: function () {
        var url = 'CaseRetrieve';
        var request = {
            CASE: {
                SN: $('#section_retrieve #SN').val(),
                NAME1: $('#section_retrieve #NAME1').val(),
                TEL1: $('#section_retrieve #TEL1').val(),
                TEACHER1: $('#section_retrieve #TEACHER1').val(),
                YEARS_OLD: $('#section_retrieve #YEARS_OLD').val(),
                ADDRESS: $('#section_retrieve #ADDRESS').val(),
                FAMILY: $('#section_retrieve #FAMILY').val(),
                RESIDENCE: $('#section_retrieve #RESIDENCE').val(),
                NOTE: $('#section_retrieve #NOTE').val(),
                PROBLEM: $('#section_retrieve #PROBLEM').val(),
                EXPERIENCE: $('#section_retrieve #EXPERIENCE').val(),
                SUGGEST: $('#section_retrieve #SUGGEST').val()
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
                        $.each(response.CASE, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-id-card-o fa-lg" onclick="MergeIndex.RowSelected(' + idx + ');"data-toggle="tooltip" data-placement="right" title="加入" style="cursor: cell;"></a></td>';
                            htmlRow += '<td>' + row.SN + '</td>';
                            htmlRow += '<td>' + row.NAME1 + row.NAME2 + row.NAME3 + row.NAME4 + row.NAME5 + row.NAME6 + '</td>';
                            //                     htmlRow += '<td>' + row.NAME1 + '</td>';
                            //htmlRow += '<td>' + row.NAME2 + '</td>';
                            //htmlRow += '<td>' + row.NAME3 + '</td>';
                            //htmlRow += '<td>' + row.NAME4 + '</td>';
                            //htmlRow += '<td>' + row.NAME5 + '</td>';
                            //htmlRow += '<td>' + row.NAME6 + '</td>';
                            htmlRow += '<td>' + row.TEL1 + row.TEL2 + row.TEL3 + row.TEL4 + row.TEL5 + row.TEL6 + '</td>';
                            //                     htmlRow += '<td>' + row.TEL1 + '</td>';
                            //htmlRow += '<td>' + row.TEL2 + '</td>';
                            //htmlRow += '<td>' + row.TEL3 + '</td>';
                            //htmlRow += '<td>' + row.TEL4 + '</td>';
                            //htmlRow += '<td>' + row.TEL5 + '</td>';
                            //htmlRow += '<td>' + row.TEL6 + '</td>';
                            htmlRow += '<td>' + row.TEACHER1 + row.TEACHER2 + row.TEACHER3 + '</td>';
                            //htmlRow += '<td>' + row.TEACHER1 + '</td>';
                            //htmlRow += '<td>' + row.TEACHER2 + '</td>';
                            //htmlRow += '<td>' + row.TEACHER3 + '</td>';
                            htmlRow += '<td>' + row.YEARS_OLD + '</td>';
                            htmlRow += '<td>' + row.ADDRESS + '</td>';
                            htmlRow += '<td>' + row.FAMILY + '</td>';
                            htmlRow += '<td>' + row.RESIDENCE + '</td>';
                            htmlRow += '<td>' + row.NOTE + '</td>';
                            htmlRow += '<td>' + row.PROBLEM + '</td>';
                            htmlRow += '<td>' + row.EXPERIENCE + '</td>';
                            htmlRow += '<td>' + row.SUGGEST + '</td>';
                            htmlRow += '</tr>';
                            $('#gridview >  tbody').append(htmlRow);
                        });
                    }
                    else {
                        htmlRow = '<tr><td colspan="12" style="text-align:center">data not found</td></tr>';
                        $('#gridview >  tbody').append(htmlRow);
                    }

                    MergeIndex.CASE = response.CASE;
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

    CaseMerge: function () {
        var url = '/Merge/CaseMerge';

        var section_modify = $('#section_modify');
        var master = {
            ADD: {
                SN: parseInt(section_modify.find('#SN_ADD').val(), 10),
                NAME1: section_modify.find('#NAME1').val(),
                NAME2: section_modify.find('#NAME2').val(),
                NAME3: section_modify.find('#NAME3').val(),
                NAME4: section_modify.find('#NAME4').val(),
                NAME5: section_modify.find('#NAME5').val(),
                NAME6: section_modify.find('#NAME6').val(),
                TEL1: section_modify.find('#TEL1').val(),
                TEL2: section_modify.find('#TEL2').val(),
                TEL3: section_modify.find('#TEL3').val(),
                TEL4: section_modify.find('#TEL4').val(),
                TEL5: section_modify.find('#TEL5').val(),
                TEL6: section_modify.find('#TEL6').val(),
                YEARS_OLD: section_modify.find('#YEARS_OLD').val(),
                ADDRESS: section_modify.find('#ADDRESS').val(),
                TEACHER1: section_modify.find('#TEACHER1').val(),
                TEACHER2: section_modify.find('#TEACHER2').val(),
                TEACHER3: section_modify.find('#TEACHER3').val(),
                FAMILY: section_modify.find('#FAMILY').val(),
                FAMILY_FILE: section_modify.find('#FAMILY_FILE:checked').val(),
                RESIDENCE: section_modify.find('#RESIDENCE').val(),
                NOTE: section_modify.find('#NOTE').val(),
                PROBLEM: section_modify.find('#PROBLEM').val(),
                EXPERIENCE: section_modify.find('#EXPERIENCE').val(),
                SUGGEST: section_modify.find('#SUGGEST').val(),
                MERGE_REASON: section_modify.find('#MERGE_REASON').val(),
            },
            REMOVE: {
                SN: parseInt(section_modify.find('#SN_REMOVE').val(), 10)
            }
        };

        var fd = new FormData();

        var jsonMaster = JSON.stringify(master)
        console.log(jsonMaster);
        fd.append('data', jsonMaster);

        //var files = $("#file1").get(0).files;
        //if (files.length > 0) {
        //    fd.append("file", files[0]);
        //}

        $.ajax({
            type: 'post',
            url: url,
            data: fd,
            processData: false,
            contentType: false,
            success: function (data) {
                console.log(data);
                var response = JSON.parse(data);
                if (response.ReturnStatus.Code === 0) {
                    //MergeIndex.CaseQuery(response.CASE.SN);
                    MergeIndex.MERGE = [];
                    MergeIndex.SetData();
                    MergeIndex.CaseRetrieve();
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

    RowSelected: function (idx) {
        var section_modify = $('#section_modify');
        if (MergeIndex.MERGE.length < 2) {
            var added = false;
            $.each(MergeIndex.MERGE, function (index, value) {
                if (MergeIndex.MERGE[index].SN === MergeIndex.CASE[idx].SN) {
                    added = true;
                    return false;
                }
            });

            if (!added) {
                MergeIndex.MERGE.push(MergeIndex.CASE[idx]);
                MergeIndex.SetData();
            }
            else {
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>序號 : ' + MergeIndex.CASE[idx].SN+' 已加入</p>');
                $('#modal').modal('show');
            }
        }
        else {
            $('#modal .modal-title').text('交易訊息');
            $('#modal .modal-body').html('<p>待合併資料最多兩筆</p>');
            $('#modal').modal('show');
        }
        //console.log(this.MERGE);
    },

    SetData: function () {
        var section_modify = $('#section_modify');
        var index;
        for (index = 0; index < 2; index++) {
            section_modify.find('#undo' + index).val('');
            section_modify.find('#undo' + index).hide();
            section_modify.find('label[for=SN' + index + ']').text('');
            section_modify.find('label[for=NAME1' + index + ']').text('');
            section_modify.find('label[for=NAME2' + index + ']').text('');
            section_modify.find('label[for=NAME3' + index + ']').text('');
            section_modify.find('label[for=NAME4' + index + ']').text('');
            section_modify.find('label[for=NAME5' + index + ']').text('');
            section_modify.find('label[for=NAME6' + index + ']').text('');
            section_modify.find('label[for=TEL1' + index + ']').text('');
            section_modify.find('label[for=TEL2' + index + ']').text('');
            section_modify.find('label[for=TEL3' + index + ']').text('');
            section_modify.find('label[for=TEL4' + index + ']').text('');
            section_modify.find('label[for=TEL5' + index + ']').text('');
            section_modify.find('label[for=TEL6' + index + ']').text('');
            section_modify.find('label[for=TEACHER1' + index + ']').text('');
            section_modify.find('label[for=TEACHER2' + index + ']').text('');
            section_modify.find('label[for=TEACHER3' + index + ']').text('');
            section_modify.find('label[for=YEARS_OLD' + index + ']').text('');
            section_modify.find('label[for=ADDRESS' + index + ']').text('');
            section_modify.find('label[for=FAMILY' + index + ']').text('');
            section_modify.find('#FAMILY_FILE' + index).val('');
            section_modify.find('label[for=FAMILY_FILE' + index + ']').find('img').attr('src', '/img/empty.png');
            section_modify.find('label[for=RESIDENCE' + index + ']').text('');
            section_modify.find('label[for=NOTE' + index + ']').text('');
            section_modify.find('label[for=PROBLEM' + index + ']').text('');
            section_modify.find('label[for=EXPERIENCE' + index + ']').text('');
            section_modify.find('label[for=SUGGEST' + index + ']').text('');
            section_modify.find('label[for=MERGE_REASON' + index + ']').text('');
        }

        $.each(MergeIndex.MERGE, function (index, value) {
            section_modify.find('#undo' + index).val(value.SN);
            section_modify.find('#undo' + index).show();
            section_modify.find('label[for=SN' + index + ']').text(value.SN);
            section_modify.find('label[for=NAME1' + index + ']').text(value.NAME1);
            section_modify.find('label[for=NAME2' + index + ']').text(value.NAME2);
            section_modify.find('label[for=NAME3' + index + ']').text(value.NAME3);
            section_modify.find('label[for=NAME4' + index + ']').text(value.NAME4);
            section_modify.find('label[for=NAME5' + index + ']').text(value.NAME5);
            section_modify.find('label[for=NAME6' + index + ']').text(value.NAME6);
            section_modify.find('label[for=TEL1' + index + ']').text(value.TEL1);
            section_modify.find('label[for=TEL2' + index + ']').text(value.TEL2);
            section_modify.find('label[for=TEL3' + index + ']').text(value.TEL3);
            section_modify.find('label[for=TEL4' + index + ']').text(value.TEL4);
            section_modify.find('label[for=TEL5' + index + ']').text(value.TEL5);
            section_modify.find('label[for=TEL6' + index + ']').text(value.TEL6);
            section_modify.find('label[for=TEACHER1' + index + ']').text(value.TEACHER1);
            section_modify.find('label[for=TEACHER2' + index + ']').text(value.TEACHER2);
            section_modify.find('label[for=TEACHER3' + index + ']').text(value.TEACHER3);
            section_modify.find('label[for=YEARS_OLD' + index + ']').text(value.YEARS_OLD);
            section_modify.find('label[for=ADDRESS' + index + ']').text(value.ADDRESS);
            section_modify.find('label[for=FAMILY' + index + ']').text(value.FAMILY);
            if (value.FAMILY_FILE) {
                section_modify.find('#FAMILY_FILE' + index).val(value.FAMILY_FILE);
                section_modify.find('label[for=FAMILY_FILE' + index + ']').find('img').attr('src', '/img/upload/' + value.FAMILY_FILE);
            }
            else {
                section_modify.find('#FAMILY_FILE' + index).val('');
                section_modify.find('label[for=FAMILY_FILE' + index + ']').find('img').attr('src', '/img/empty.png');
            }
            section_modify.find('label[for=RESIDENCE' + index + ']').text(value.RESIDENCE);
            section_modify.find('label[for=NOTE' + index + ']').text(value.NOTE);
            section_modify.find('label[for=PROBLEM' + index + ']').text(value.PROBLEM);
            section_modify.find('label[for=EXPERIENCE' + index + ']').text(value.EXPERIENCE);
            section_modify.find('label[for=SUGGEST' + index + ']').text(value.SUGGEST);
            section_modify.find('label[for=MERGE_REASON' + index + ']').text(value.MERGE_REASON);
        });

        if (MergeIndex.MERGE.length === 2) {

            section_modify.find('input[type=radio][name=SN]').click(function () {
                var combin = '';
                var remove = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#SN' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=SN' + index + ']').text();
                    }
                    else {
                        remove += section_modify.find('label[for=SN' + index + ']').text();
                    }
                });
                section_modify.find('#SN_ADD').val(combin);
                section_modify.find('#SN_REMOVE').val(remove);
            });

            section_modify.find('input[type=checkbox][name=NAME1]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#NAME1' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=NAME1' + index + ']').text();
                    }
                });
                section_modify.find('#NAME1').val(combin);
            });
            section_modify.find('input[type=checkbox][name=NAME2]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#NAME2' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=NAME2' + index + ']').text();
                    }
                });
                section_modify.find('#NAME2').val(combin);
            });
            section_modify.find('input[type=checkbox][name=NAME3]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#NAME3' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=NAME3' + index + ']').text();
                    }
                });
                section_modify.find('#NAME3').val(combin);
            });
            section_modify.find('input[type=checkbox][name=NAME4]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#NAME4' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=NAME4' + index + ']').text();
                    }
                });
                section_modify.find('#NAME4').val(combin);
            });
            section_modify.find('input[type=checkbox][name=NAME5]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#NAME5' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=NAME5' + index + ']').text();
                    }
                });
                section_modify.find('#NAME5').val(combin);
            });
            section_modify.find('input[type=checkbox][name=NAME6]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#NAME6' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=NAME6' + index + ']').text();
                    }
                });
                section_modify.find('#NAME6').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEL1]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEL1' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEL1' + index + ']').text();
                    }
                });
                section_modify.find('#TEL1').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEL2]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEL2' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEL2' + index + ']').text();
                    }
                });
                section_modify.find('#TEL2').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEL3]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEL3' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEL3' + index + ']').text();
                    }
                });
                section_modify.find('#TEL3').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEL4]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEL4' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEL4' + index + ']').text();
                    }
                });
                section_modify.find('#TEL4').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEL5]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEL5' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEL5' + index + ']').text();
                    }
                });
                section_modify.find('#TEL5').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEL6]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEL6' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEL6' + index + ']').text();
                    }
                });
                section_modify.find('#TEL6').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEACHER1]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEACHER1' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEACHER1' + index + ']').text();
                    }
                });
                section_modify.find('#TEACHER1').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEACHER2]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEACHER2' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEACHER2' + index + ']').text();
                    }
                });
                section_modify.find('#TEACHER2').val(combin);
            });
            section_modify.find('input[type=checkbox][name=TEACHER3]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#TEACHER3' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=TEACHER3' + index + ']').text();
                    }
                });
                section_modify.find('#TEACHER3').val(combin);
            });
            section_modify.find('input[type=checkbox][name=YEARS_OLD]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#YEARS_OLD' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=YEARS_OLD' + index + ']').text();
                    }
                });
                section_modify.find('#YEARS_OLD').val(combin);
            });
            section_modify.find('input[type=checkbox][name=ADDRESS]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#ADDRESS' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=ADDRESS' + index + ']').text();
                    }
                });
                section_modify.find('#ADDRESS').val(combin);
            });
            section_modify.find('input[type=checkbox][name=FAMILY]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#FAMILY' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=FAMILY' + index + ']').text();
                    }
                });
                section_modify.find('#FAMILY').val(combin);
            });

            section_modify.find('input[type=radio][name=FAMILY_FILE]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#FAMILY_FILE' + index + ':checked').val()) {
                        combin += section_modify.find('#FAMILY_FILE' + index).val();
                    }
                });
                if (combin) {
                    section_modify.find('#FAMILY_FILE').val(combin);
                    section_modify.find('img[name=FAMILY_FILE]').attr('src', '/img/upload/' + combin);
                }
                else {
                    section_modify.find('#FAMILY_FILE').val('');
                    section_modify.find('img[name=FAMILY_FILE]').attr('src', '/img/empty.png' );
                }
            });

            section_modify.find('input[type=checkbox][name=RESIDENCE]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#RESIDENCE' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=RESIDENCE' + index + ']').text();
                    }
                });
                section_modify.find('#RESIDENCE').val(combin);
            });
            section_modify.find('input[type=checkbox][name=NOTE]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#NOTE' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=NOTE' + index + ']').text();
                    }
                });
                section_modify.find('#NOTE').val(combin);
            });
            section_modify.find('input[type=checkbox][name=PROBLEM]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#PROBLEM' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=PROBLEM' + index + ']').text();
                    }
                });
                section_modify.find('#PROBLEM').val(combin);
            });
            section_modify.find('input[type=checkbox][name=EXPERIENCE]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#EXPERIENCE' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=EXPERIENCE' + index + ']').text();
                    }
                });
                section_modify.find('#EXPERIENCE').val(combin);
            });
            section_modify.find('input[type=checkbox][name=SUGGEST]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#SUGGEST' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=SUGGEST' + index + ']').text();
                    }
                });
                section_modify.find('#SUGGEST').val(combin);
            });
            section_modify.find('input[type=checkbox][name=MERGE_REASON]').click(function () {
                var combin = '';
                $.each(MergeIndex.MERGE, function (index, value) {
                    if (section_modify.find('#MERGE_REASON' + index + ':checked').val()) {
                        combin += section_modify.find('label[for=MERGE_REASON' + index + ']').text();
                    }
                });
                section_modify.find('#MERGE_REASON').val(combin);
            });
        }
        else {
            section_modify.find('input[type=checkbox]').unbind("click");
            section_modify.find('input[type=radio]').unbind("click");
            section_modify.find('input[type=checkbox]').prop('checked',false)
            section_modify.find('input[type=radio]').prop('checked', false)

            section_modify.find('#SN_ADD').val('');
            section_modify.find('#SN_REMOVE').val('');
            section_modify.find('#NAME1').val('');
            section_modify.find('#NAME2').val('');
            section_modify.find('#NAME3').val('');
            section_modify.find('#NAME4').val('');
            section_modify.find('#NAME5').val('');
            section_modify.find('#NAME6').val('');
            section_modify.find('#TEL1').val('');
            section_modify.find('#TEL2').val('');
            section_modify.find('#TEL3').val('');
            section_modify.find('#TEL4').val('');
            section_modify.find('#TEL5').val('');
            section_modify.find('#TEL6').val('');
            section_modify.find('#TEACHER1').val('');
            section_modify.find('#TEACHER2').val('');
            section_modify.find('#TEACHER3').val('');
            section_modify.find('#YEARS_OLD').val('');
            section_modify.find('#ADDRESS').val('');
            section_modify.find('#FAMILY').val('');
            section_modify.find('#FAMILY_FILE').val('');
            section_modify.find('label[for=FAMILY_FILE]').find('img').attr('src', '/img/empty.png');
            section_modify.find('#RESIDENCE').val('');
            section_modify.find('#NOTE').val('');
            section_modify.find('#PROBLEM').val('');
            section_modify.find('#EXPERIENCE').val('');
            section_modify.find('#SUGGEST').val('');
            section_modify.find('#MERGE_REASON').val('');
        }

    },

    DataValidate: function () {
        var section_modify = $('#section_modify');

        $('#section_modify').validate({
            rules: {
                SN_ADD: { required: true },
                _NAME1: {
                    required: function (element) {
                        return !(section_modify.find('input[type=text][name=_NAME2]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME3]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME4]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME5]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME6]').val().length > 0);
                    },
                },
                _NAME2: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_NAME1]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME3]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME4]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME5]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME6]').val().length > 0);
                    },
                },
                _NAME3: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_NAME1]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME2]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME4]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME5]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME6]').val().length > 0);
                    },
                },
                _NAME4: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_NAME1]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME2]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME3]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME5]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME6]').val().length > 0);
                    },
                },
                _NAME5: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_NAME1]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME2]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME3]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME4]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME6]').val().length > 0);
                    },
                },
                _NAME6: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_NAME1]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME2]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME3]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME4]').val().length > 0
                            || section_modify.find('input[type=text][name=_NAME5]').val().length > 0);
                    },
                },
                _TEL1: {
                    required: function (element) {
                        return !(section_modify.find('input[type=text][name=_TEL2]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL3]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL4]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL5]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL6]').val().length > 0);
                    },
                },
                _TEL2: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_TEL1]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL3]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL4]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL5]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL6]').val().length > 0);
                    },
                },
                _TEL3: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_TEL1]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL2]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL4]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL5]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL6]').val().length > 0);
                    },
                },
                _TEL4: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_TEL1]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL2]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL3]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL5]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL6]').val().length > 0);
                    },
                },
                _TEL5: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_TEL1]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL2]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL3]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL4]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL6]').val().length > 0);
                    },
                },
                _TEL6: {
                    required: function (element) {
                        return !(
                            section_modify.find('input[type=text][name=_TEL1]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL2]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL3]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL4]').val().length > 0
                            || section_modify.find('input[type=text][name=_TEL5]').val().length > 0);
                    },
                },
                //AGE: 'required',
                //ADDRESS: 'required',
                //FAMILY: 'required'
                //EXPERIENCE: 'required',
                //SUGGEST: 'required',
            },
            messages:
                {
                    _NAME1: "稱謂1~6至少輸入一個",
                    _NAME2: "稱謂1~6至少輸入一個",
                    _NAME3: "稱謂1~6至少輸入一個",
                    _NAME4: "稱謂1~6至少輸入一個",
                    _NAME5: "稱謂1~6至少輸入一個",
                    _NAME6: "稱謂1~6至少輸入一個",
                    _TEL1: "電話1~6至少輸入一個",
                    _TEL2: "電話1~6至少輸入一個",
                    _TEL3: "電話1~6至少輸入一個",
                    _TEL4: "電話1~6至少輸入一個",
                    _TEL5: "電話1~6至少輸入一個",
                    _TEL6: "電話1~6至少輸入一個",
                },
            ignore: false,
            focusInvalid: false,
            invalidHandler: function (form, validator) {
                if (!validator.numberOfInvalids())
                    return;
                $('html, body').animate({
                    scrollTop: $(validator.errorList[0].element).offset().top - 130
                }, 200);
            }
        });
    },

    ValueRecover: function (action) {
        if (action === 'U') {
            var section_modify = $("#section_modify")
            MergeIndex.CaseQuery(section_modify.find('[name=SN]').val());
        }
        else {
            var section_modify = $("#section_modify")
            section_modify.validate().resetForm();
            var master_sectoin = section_modify.find('#master_sectoin');
            master_sectoin.find('.fields').each(function () {
                $(this).val('');
            });
            document.getElementById('file1').value = '';
            master_sectoin.find('[name=FAMILY_FILE]').attr('src', '');
            $('#detail_section').html('');


        }
    },

    FileUrl: function (file) {
        var url = null;
        if (window.createObjcectURL != undefined) {
            url = window.createOjcectURL(file);
        } else if (window.URL != undefined) {
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) {
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    },

};
