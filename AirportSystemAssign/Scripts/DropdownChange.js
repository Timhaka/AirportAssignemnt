function preventDouble(select, index) {
    var options = select.options,
        len = options.length;
    while (len--) {
        options[len].disabled = false;
    }
    select.options[index].disabled = true;
}

var select1 = select = document.getElementById('select1');
var select2 = select = document.getElementById('select2');

select1.onchange = function () {
    preventDouble.call(this, select2, this.selectedIndex);
};

select2.onchange = function () {
    preventDouble.call(this, select1, this.selectedIndex);
};

