$(document).ready(function () {


    if (typeof (window.FileReader) == 'undefined') {
        alert('Drag&Drop не поддерживается в вашем браузере.');
    }

    var filesArea = $(".files-area");
    filesArea.on('drag dragstart dragend dragover dragenter dragleave drop', function (e) {
            e.preventDefault();
            e.stopPropagation();
        })
        .on('dragover dragenter', function () {
            filesArea.addClass('is-dragover');
        })
        .on('dragleave dragend drop', function () {
            filesArea.removeClass('is-dragover');
        })
        .on('drop', function (e) {
            var files = e.originalEvent.dataTransfer.files;
            addFiles(files, filesArea);
        });

    function getSizeFiles(files) {
        var size = 0;
        for (var i = 0; i < files.length; i++) {
            size += files[i].size;
        }

        return size;
    }

    function getSizeAttachFiles(newFileSize) {
        var size = 0;
        $('.attach-file').each(function () {
            size += $(this)[0].file.size;
        });

        if (!newFileSize)
            return size;
        else
            return size + newFileSize;
    }

    var currentFileId = 0;

    function addFiles(files, filesArea) {

        if (files.length > 15) {
            alert('Превышено максимальное количество загружаемых файлов (15 шт.)');
            return;
        }
        var currentFilesSize = getSizeFiles(files);
        // Новые файлы весят не больше 30 Мб
        if (currentFilesSize > 31457280) {
            alert('Превышен максимальный обьем загружаемых файлов (30Мб)');
            return;
        }
        // Новые файлы + старые файлы весят е более 30 Мб
        else if (getSizeAttachFiles(currentFilesSize) > 31457280) {
            alert('Превышен максимальный обьем загружаемых файлов (30Мб)');
            return;
        }

        for (var i = 0; i < files.length; i++) {
            filesArea.append('<div class="attach-file" data-id="' + currentFileId + '"><i class="glyphicon glyphicon-file"></i><div class="file-name">' + files[i].name + '</div><i class="glyphicon glyphicon-remove remove-file"></span></div>');
            $('.attach-file[data-id=' + currentFileId + ']')[0].file = files[i];
            currentFileId++;

            $('.remove-file').click(function () {
                var attach = $(this).parent();
                attach.remove();
            });
        }
    }

    // Выбор файла
    $('#files').change(function () {

        var files = $(this)[0].files;
        if (getSizeFiles(files) > 31457280) {
            alert('Превышен максимальный обьем загружаемых файлов (30Мб)');
            return;
        }
        addFiles(files, filesArea);
    });

    // Кнопка "Загрузить"
    $('#upload-files-btn').click(function () {
        $('#upload-files-form').submit();
    });

    // Загрузка фалов на сервер
    $('#upload-files-form').submit(function (e) {
        e.preventDefault();
        // Показ лоадера
        $('#loader-upload-file').show();
        var formData = new FormData($(this)[0]);

        // Добавление файлов к форме
        $('.attach-file').each(function () {
            currentFile = $(this)[0].file;
            formData.append('files', currentFile);
        });

        $.ajax({
            url: "/File/Upload",
            //url: "http://127.0.0.1:5001/api/file",
            type: "POST",
            data: formData, //serialize(),
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
            async: true,
            success: function (response) {
                // Скрыть лоадер
                $('#loader-upload-file').hide();
                // Показать результаты
                $('#upload-file-result').html(response);
                initListenersTable();
            },
            error: function (response) {
                // Скрыть лоадер
                $('#loader-upload-file').hide();
                // Вывод сообщения об ошибке
                alert("Status: " + response.status + " - " + response.statusText);
            },
            failure: function (response) {
                $('#loader-upload-file').hide();
                alert("Status: " + response.status + " - " + response.statusText);
            }
        });
    });

    // Отправка формы поиска файлов
    $('#find-file-form').submit(function (e) {
        e.preventDefault();
        var select = $(this).find('input[name="select"]').val();
        // Показ лоадера
        $('#loader-find-file').show();
        $.ajax({
            url: "/File/Find",
            type: "POST",
            data: {
                select: select
            },
            async: true,
            success: function (response) {
                $('#loader-find-file').hide();
                $('#find-file-result').html(response);
                initListenersTable();
            },
            error: function (response) {
                // Скрыть лоадер
                $('#loader-find-file').hide();
                // Вывести сообщение об ошибке
                alert("Status: " + response.status + " - " + response.statusText);
            },
            failure: function (response) {
                // Скрыть лоадер
                $('#loader-find-file').hide();
                // Вывести сообщение об ошибке
                alert("Status: " + response.status + " - " + response.statusText);
            }
        });
    });

    // Привязать события удаления, скачивания и изменения файлов в таблице
    function initListenersTable() {
        $('table tr td .glyphicon-download-alt').click(function (e) {
            // Показ лоадера
            $('#loader-find-file').show();
            // Получение текущей строки
            var currentRow = $(this).parent().parent();
            // Получение id файла
            var id = currentRow.find('.file-id').text();
            // Получить файл
            window.location.href = "/File/Download/" + id;
            // Скрыть лоадер
            $('#loader-find-file').hide();
        });

        $('table tr td .glyphicon-remove').click(function (e) {
            // Показ лоадера
            $('#loader-find-file').show();
            // Получение текущей строки
            var currentRow = $(this).parent().parent();
            // Получение id файла
            var id = currentRow.find('.file-id').text();
            $.ajax({
                url: "/File/Delete",
                type: "POST",
                data: {
                    id: id
                },
                async: true,
                success: function (response) {
                    // Скрыть лоадер
                    $('#loader-find-file').hide();
                    if (response.error)
                        alert(response.error);
                    else if (response.success) {
                        alert("Удалено файлов: " + response.success);
                        currentRow.remove();
                    } else
                        alert(response);
                },
                failure: function (response) {
                    // Скрыть лоадер
                    $('#loader-find-file').hide();
                    alert("Status: " + response.status + " - " + response.statusText);
                },
                error: function (response) {
                    // Скрыть лоадер
                    $('#loader-find-file').hide();
                    alert("Status: " + response.status + " - " + response.statusText);
                }
            });
        });

        $('table tr td .glyphicon-edit').click(function (e) {
            // Получение текущей строки
            var currentRow = $(this).closest('tr');
            // Получение id файла
            var $id = currentRow.find('.file-id');
            // Получение названия файла
            var $name = currentRow.find('.file-name');
            // Получение описание файла
            var $description = currentRow.find('.file-description');
            // Получение имени файла
            var $filename = currentRow.find('.file-filename');

            // Модальное окно изменения файла
            var modal = $('#update-file-modal');
            // Заполнить поля модального окна данными из таблицы
            modal.find('input[name="id"]').val($id.text());
            modal.find('input[name="name"]').val($name.text());
            modal.find('textarea[name="description"]').val($description.text());
            modal.find('input[name="file-name"]').val($filename.text());
            // Показать модальное окно
            modal.modal('show');

            // Сохранение изменений файла  модальном окне
            $('#update-file-modal .btn-primary').click(function (e) {
                e.preventDefault();
                // Показать лоадер
                $('#loader-opdate-file').show();
                $.ajax({
                    url: "/File/Update",
                    type: "POST",
                    data: modal.find('form').serialize(),
                    async: true,
                    success: function (response) {
                        // Скрыть лоадер
                        $('#loader-opdate-file').hide();
                        if (response.error)
                            alert(response.error);
                        else if (response.success) {
                            // Выводим сообщение
                            //alert('Изменено файлов: ' + response.success);
                            // Закрываем модальное окно
                            modal.modal('hide');
                            // Обновляем результат поиска !!!!!
                            $id.text(modal.find('input[name="id"]').val());
                            $name.text(modal.find('input[name="name"]').val());
                            $description.text(modal.find('textarea[name="description"]').val());
                            $filename.text(modal.find('input[name="file-name"]').val());

                        } else
                            alert(response);
                    },
                    failure: function (response) {
                        // Скрыть лоадер
                        $('#loader-find-file').hide();
                        alert("Status: " + response.status + " - " + response.statusText);
                    },
                    error: function (response) {
                        // Скрыть лоадер
                        $('#loader-find-file').hide();
                        alert("Status: " + response.status + " - " + response.statusText);
                    }
                });
            });
        });
    }



});