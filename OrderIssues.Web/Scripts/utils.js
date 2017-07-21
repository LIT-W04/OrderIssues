function formatMVCDate(mvcDate) {
    var date = new Date(parseInt(mvcDate.substr(6)));
    return moment(date).format('MM/DD/YYYY');
}