export const getFormattedDate = (date) => {
    date = String(date).split('T');
    var days = String(date[0]).split('-');
    var hours = String(date[1]).split(':');
    return date[0] + " " + parseInt(hours[0]) + ":" + parseInt(hours[1]) + ":" + parseInt(hours[2]);
}
