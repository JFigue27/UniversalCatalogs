import moment from 'moment';
export default class UtilsService {
  toJavascriptDate(iso_8601_date) {
    return iso_8601_date ? moment(iso_8601_date, moment.ISO_8601).toDate() : null;
  }

  toServerDate(date) {
    let momentDate = moment(date);
    if (momentDate.isValid()) {
      momentDate.local();
      return momentDate.format();
    }
    return null;
  }
}
