$(document).ready(function () {

    function checkFileSize(file) {
        // Максимальный размер - 30Мб
        if (file.size > 30000000) {
            alert('Превышен максимальный обьем загружаемых файлов (30Мб)');
            return false;
        }
        return true;
    }

    // Выбрали текст 1 из файла
    $('#file-text-one').change(function () {
        var file = $(this)[0].files[0]; // Получение выбранных файлов

        if (!checkFileSize(file))
            return;

        // Скрываем textarea и показываем файл
        $('#text-one').hide();
        $('#file-text-one-box .file-name').text(file.name);
        $('#file-text-one-box').fadeIn(100);

    });

    // Удалить файл 1
    $('#file-text-one-box .remove-file').click(function () {
        $('#file-text-one').val("");
        $('#file-text-one-box').hide();
        $('#text-one').fadeIn(100);
    });

    // Выбрали текст 2 из файла
    $('#file-text-two').change(function () {
        var file = $(this)[0].files[0]; // Получение выбранных файлов

        if (!checkFileSize(file))
            return;

        // Скрываем textarea и показываем файл
        $('#text-two').hide();
        $('#file-text-two-box .file-name').text(file.name);
        $('#file-text-two-box').fadeIn(100);

    });

    // Удалить файл 2
    $('#file-text-two-box .remove-file').click(function () {
        $('#file-text-two').val("");
        $('#file-text-two-box').hide();
        $('#text-two').fadeIn(100);
    });

    // Получение формы для отправки на сравнение двух текстов
    function getDataToCmpTwoTexts() {
        var textOne = $('#text-one').val();
        var textTwo = $('#text-two').val();
        var fileTextOne = $('#file-text-one')[0].files[0];
        var fileTextTwo = $('#file-text-two')[0].files[0];

        var formData = new FormData();

        // Если первый файл выбран, то отправляем его
        if (fileTextOne != undefined)
            formData.append('fileTextOne', fileTextOne);
        else if (textOne != "") // иначе - отправляем первый текст
            formData.append('textOne', textOne);
        else {
            alert('Не выбран первый текст для сравнения');
            return null;
        }

        // Аналогично со вторым текстом
        if (fileTextTwo != undefined)
            formData.append('fileTextTwo', fileTextTwo);
        else if (textTwo != "")
            formData.append('textTwo', textTwo);
        else {
            alert('Не выбран второй текст для сравнения');
            return null;
        }

        return formData;
    }


    // Получение формы для отправки на сравнение двух текстов
    function getDataToCmpTextWithBase() {
        var textOne = $('#text-one').val();
        var fileTextOne = $('#file-text-one')[0].files[0];

        var formData = new FormData();

        // Если первый файл выбран, то отправляем его
        if (fileTextOne != undefined)
            formData.append('fileTextOne', fileTextOne);
        else if (textOne != "") // иначе - отправляем первый текст
            formData.append('textOne', textOne);
        else {
            alert('Не выбран первый текст для сравнения');
            return null;
        }

        return formData;
    }
    // Лоадер
    var $loader = $('#loader-cmp-two-text');
    Timer.init($loader);



    /////////////////////////////////////////////////
    // Сравнение двух текстов CRC32
    $('#btn-crc32').click(function (e) {
        e.preventDefault();

        var data = getDataToCmpTwoTexts();
        if (data == null)
            return;
        $loader.show();
        $.ajax({
            url: "/Compare/CompareTwoTexts",
            type: 'POST',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
            async: true,
            error: isError,
            success: isSuccess,
            failure: isError
        });
        return false;

        function isSuccess(response) {
            $loader.hide();
            var data = JSON.parse(response);
            if (data.error) { // Ошибка
                alert("Ошибка: " + data.error);
                return;
            } else if (data.success) {
                $('#cmp-two-result-field').show();
                $('#cmp-two-result').text(data.success + "%");
                alert('Время сравнения: ' + data.timeSpan / 1000 + ' сек.');
            } else {
                alert("Error: Empty response");
                return;
            }
        }

        function isError(response) {
            $loader.hide();
            alert("Status: " + response.status + " - " + response.statusText);
        }

    });


    $('#btn-random-hash').click(function (e) {
        e.preventDefault();

        var data = getDataToCmpTwoTexts();
        if (data == null)
            return;
        $loader.show();
        $.ajax({
            url: "/Compare/CompareTwoTextsRandom",
            type: 'POST',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
            async: true,
            error: isError,
            success: isSuccess,
            failure: isError
        });
        return false;

        function isSuccess(response) {
            $loader.hide();
            var data = JSON.parse(response);
            if (data.error) { // Ошибка
                alert("Ошибка: " + data.error);
                return;
            } else if (data.success) {
                $('#cmp-two-result-field').show();
                $('#cmp-two-result').text(data.success + "%");
                alert('Время сравнения: ' + data.timeSpan / 1000 + ' сек.');
            } else {
                alert("Error: Empty response");
                return;
            }
        }

        function isError(response) {
            $loader.hide();
            alert("Status: " + response.status + " - " + response.statusText);
        }

    });

    $('#btn-max-hash').click(function (e) {
        e.preventDefault();

        var data = getDataToCmpTwoTexts();
        if (data == null)
            return;
        $loader.show();
        $.ajax({
            url: "/Compare/CompareTwoTextsMax",
            type: 'POST',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
            async: true,
            error: isError,
            success: isSuccess,
            failure: isError
        });
        return false;

        function isSuccess(response) {
            $loader.hide();
            var data = JSON.parse(response);
            if (data.error) { // Ошибка
                alert("Ошибка: " + data.error);
                return;
            } else if (data.success) {
                $('#cmp-two-result-field').show();
                $('#cmp-two-result').text(data.success + "%");
                alert('Время сравнения: ' + data.timeSpan / 1000 + ' сек.');
            } else {
                alert("Error: Empty response");
                return;
            }
        }

        function isError(response) {
            $loader.hide();
            alert("Status: " + response.status + " - " + response.statusText);
        }

    });


    $('#btn-min-hash').click(function (e) {
        e.preventDefault();

        var data = getDataToCmpTwoTexts();
        if (data == null)
            return;
        $loader.show();
        $.ajax({
            url: "/Compare/CompareTwoTextsMin",
            type: 'POST',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
            async: true,
            error: isError,
            success: isSuccess,
            failure: isError
        });
        return false;

        function isSuccess(response) {
            $loader.hide();
            var data = JSON.parse(response);
            if (data.error) { // Ошибка
                alert("Ошибка: " + data.error);
                return;
            } else if (data.success) {
                $('#cmp-two-result-field').show();
                $('#cmp-two-result').text(data.success + "%");
                alert('Время сравнения: ' + data.timeSpan / 1000 + ' сек.');
            } else {
                alert("Error: Empty response");
                return;
            }
        }

        function isError(response) {
            $loader.hide();
            alert("Status: " + response.status + " - " + response.statusText);
        }

    });

    ///////////////////////////////////









    $('a[href="#cmp-base"]').click(function (e) {

        $('#first-text-field').removeClass('col-md-6');
        $('#second-text-field').hide();

        $('#first-text-field').addClass('col-md-offset-2 col-md-8');

        $('#compare-two-texts-btn').hide();
        $('#compare-with-base-btn').show();

        $('#cmp-with-base-result').html('');
        $('#cmp-two-result-field').hide();

    });

    $('a[href="#cmp-two"]').click(function (e) {

        $('#first-text-field').removeClass('col-md-offset-2');

        $('#first-text-field').addClass('col-md-6');
        $('#second-text-field').show();

        $('#compare-two-texts-btn').show();
        $('#compare-with-base-btn').hide();

        $('#cmp-with-base-result').html('');
        $('#cmp-two-result-field').hide();
    });


    // Сравнение текста с базой
    $('#compare-with-base-btn').click(function (e) {
        e.preventDefault();

        var data = getDataToCmpTextWithBase();
        if (data == null)
            return;
        $loader.show();
        Timer.start();
        $.ajax({
            url: "/Compare/CompareTextWithBase",
            type: 'POST',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
            async: true,
            error: isError,
            success: isSuccess,
            failure: isError
        });
        return false;

        function isSuccess(response) {
            $loader.hide();
            Timer.stop();
            if (response) {
                // Вывод результата
                $('#cmp-with-base-result').html(response);
                // Привязка событий для кнопок-фильтров
                bindFilterButtonsListeners();

            } else {
                alert("Error: Empty response");
                return;
            }
        }

        function isError(response) {
            $loader.hide();
            Timer.stop();
            alert("Status: " + response.status + " - " + response.statusText);
        }

    });

    // Привязка событий для кнопок-фильтров
    function bindFilterButtonsListeners() {
        $('#filter-buttons-group .btn').click(function () {
            // Сделать все кнопки неактивными
            $('#filter-buttons-group .btn').removeClass('active');
            // Сделать активной текущую кнопку
            $(this).addClass('active');
            // Получение названия класса, по которому будут фильтроваться результаты
            var filterClass = $(this).attr('data-class-show-name');
            // Скрыть результаты в соответствии с фильтром
            if (filterClass) {
                $('#cmp-with-base-result table tr > td:not(.' + filterClass + ')').parent().hide();
                $('#cmp-with-base-result table tr > td.' + filterClass).parent().show();
            }
            else {
                $('#cmp-with-base-result table tr > td').parent().show();
            }
        });
    }

});