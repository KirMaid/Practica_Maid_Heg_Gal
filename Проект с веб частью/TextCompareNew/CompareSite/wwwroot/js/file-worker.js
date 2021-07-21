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

    // Максимальный размер одного файла
    var MaxFileSize = 30 * 1024 * 1024;
    // Максимальное количество файлов за 1 загрузку
    var MaxFileCount = 100;
    // Максимальный объем всех файлов за один раз
    var MaxAllFilesSize = MaxFileSize * MaxFileCount;

    function addFiles(files, filesArea) {

        if (files.length > MaxFileCount) {
            alert('Превышено максимальное количество загружаемых файлов (' + MaxFileCount + ' шт.)');
            return;
        }
        var currentFilesSize = getSizeFiles(files);

        // Новые файлы + старые файлы весят не более 30 Мб
        if (getSizeAttachFiles(currentFilesSize) > MaxAllFilesSize) {
            alert('Превышен максимальный обьем загружаемых файлов (' + MaxAllFilesSize + ' байт)');
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
        // Выбранные файлы
        var files = $(this)[0].files;
        // Размер выбранных файлов
        var currentFilesSize = getSizeFiles(files);
        // Новые файлы + старые файлы весят не более 30 Мб
        if (getSizeAttachFiles(currentFilesSize) > MaxAllFilesSize) {
            alert('Превышен максимальный обьем загружаемых файлов (' + MaxAllFilesSize + ' байт)');
            return;
        }
        addFiles(files, filesArea);
    });

    // Кнопка "Загрузить"
    $('#upload-files-btn').click(function () {
        $('#upload-files-form').submit();
    });

    // Лоадер
    var $loader = $('#loader-upload-file');
    Timer.init($loader);

    // Загрузка фалов на сервер
    $('#upload-files-form').submit(function (e) {
        e.preventDefault();

        // Показ лоадера
        $loader.show();
        Timer.start();

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
                $loader.hide();
                Timer.stop();

                if (response.error) {
                    alert(response.error);
                    return;
                }

                // Показать результаты
                $('#upload-file-result').html(response);
                initListenersTable();
                // Удалить файлы из поля
                $('.attach-file').remove();
            },
            error: function (response) {
                // Скрыть лоадер
                $loader.hide();
                Timer.stop();
                // Вывод сообщения об ошибке
                alert("Status: " + response.status + " - " + response.statusText);
            },
            failure: function (response) {
                $loader.hide();
                Timer.stop();
                alert("Status: " + response.status + " - " + response.statusText);
            }
        });
    });



    // Загрузка фалов на сервер HARD
    $('#upload-files-form11').submit(function (e) {
        e.preventDefault();

        var $loader = $('#loader-upload-file');
        Timer.init($loader);

        // Показ лоадера
        $loader.show();
        Timer.start();

        var countUploads = 0;


        // КАЖДЫЙ ФАЙЛ ЗАГРУЖАЕТСЯ ОТДЕЛЬНЫМ ЗАПРОСОМ СИНХРОННО
        $('.attach-file').each(function () {
            // Флаг наступления ошибки
            var isError = false;

            var formData = new FormData($(this)[0]);
            currentFile = $(this)[0].file;
            formData.append('files', currentFile);

            var currentFile = $(this);

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
                    $('#upload-file-result table tbody').append(response);
                    currentFile.remove();
                },
                error: function (response) {
                    alert("Status: " + response.status + " - " + response.statusText);
                    isError = true;
                },
                failure: function (response) {
                    alert("Status: " + response.status + " - " + response.statusText);
                    isError = true;
                }
            });

            // Если наступает ошибка, прерываем цикл
            if (isError) {
                alert('Файл ' + currentFile.FileName + ' не загрузился')
                return;
            }

        });

        $loader.hide();
        Timer.stop();
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