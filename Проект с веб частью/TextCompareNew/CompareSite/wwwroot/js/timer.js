// Секундомер
var Timer = function () {
    // Поле с таймером
    var $timer;
    // Блок с минутами
    var $min;
    // Блок с секундами
    var $sec;

    var timerInterval;

    function init($parentField){

        $parentField.prepend('<div class="timer">Прошло: <span class="mins">0</span> мин. <span class="secs">0</span> сек.</div>');

        $timer = $parentField.find('.timer');
        // Блок с минутами
        $min = $timer.find('.mins');
        // Блок с секундами
        $sec = $timer.find('.secs');
    }

    function start() {

        var mins = 0;
        var secs = 0;

        timerInterval = setInterval(function () {
            secs++;
            if (secs == 60) {
                secs = 0;
                mins++;
            }

            $min.text(mins);
            $sec.text(secs);

        }, 1000);
    }

    function stop() {
        clearInterval(timerInterval);
    }

    return{
        init: init,
       start: start,
       stop: stop 
    }
}();