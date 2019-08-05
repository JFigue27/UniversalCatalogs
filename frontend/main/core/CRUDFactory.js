import 'isomorphic-fetch';
import moment from 'moment';
import AppConfig from './AppConfig';
import AuthService from '../core/AuthService';

const Request = async (method, url, data, BaseURL) => {
  if (AuthService.auth == null) AuthService.fillAuthData();

  const config = {
    method: method,
    mode: 'cors',
    cache: 'no-cache',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${AuthService.auth.user.BearerToken}`
    }
  };
  if (['POST', 'PUT', 'DELETE'].includes(method)) config.body = JSON.stringify(data);
  let response;
  try {
    response = await fetch((BaseURL || AppConfig.BaseURL) + url, config);
  } catch (e) {
    console.log(e);
    alert('Failed to fetch. Probably server is down.');
  }
  if (response.status == 401) throw response;
  if (response.status == 403) alert('Invalid Role.');
  return await response.json();
};

export class CRUDFactory {
  constructor(config) {
    this.config = config;
    this.EndPoint = config.EndPoint;
  }

  async InsertEntity(entity) {
    this.ADAPTER_OUT(entity);
    return await Request('POST', this.EndPoint, entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async CreateAndCheckout(entity) {
    this.ADAPTER_OUT(entity);
    return await Request('POST', this.EndPoint + '/CreateAndCheckout', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async CreateInstance(entity) {
    return await Request('POST', this.EndPoint + '/CreateInstance', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Get(operation) {
    return await Request('GET', this.EndPoint + '/' + operation).catch(this.GeneralError);
  }

  async Post(operation, entity = {}) {
    return await Request('POST', this.EndPoint + '/' + operation, entity).catch(this.GeneralError);
  }

  async GetPaged(limit, page, params = '?') {
    return await Request('GET', this.EndPoint + '/getPaged/' + limit + '/' + page + params + '&noCache=' + Number(new Date()))
      .then(r => this.UseCommonResponse(r, true))
      .catch(this.GeneralError);
  }

  async GetSingleWhere(property, value, params = '') {
    if (property && value) {
      return await Request(
        'GET',
        this.EndPoint + '/GetSingleWhere/' + property + '/' + value + '?' + params + '&noCache=' + Number(new Date())
      )
        .then(r => this.UseNudeResponse(r))
        .catch(this.GeneralError);
    } else if (params.length > 1) {
      return await Request('GET', this.EndPoint + '/GetSingleWhere?' + params + '&noCache=' + Number(new Date()))
        .then(r => this.UseNudeResponse(r))
        .catch(this.GeneralError);
    } else {
      return Promise.reject('Invalid params for GetSingleWhere.');
    }
  }

  async LoadEntities(params = '?') {
    return await Request('GET', this.EndPoint + params + '&noCache=' + Number(new Date()))
      .then(r => this.UseNudeResponse(r))
      .catch(this.GeneralError);
  }

  async LoadEntity(id) {
    if (id) {
      return await Request('GET', this.EndPoint + '/' + id)
        .then(r => this.UseNudeResponse(r))
        .catch(this.GeneralError);
    } else {
      return Promise.reject('Id not found');
    }
  }

  async Remove(entity) {
    return await Request('DELETE', this.EndPoint, entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async RemoveById(id) {
    return await Request('DELETE', this.EndPoint + '/' + id)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Save(entity) {
    if (entity.Id > 0) {
      return await this.UpdateEntity(entity);
    } else {
      return await this.InsertEntity(entity);
    }
  }

  async SendTestEmail(entity) {
    return await Request('POST', this.EndPoint + '/SendTestEmail', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async SetProperty(entity, sProperty, Value, qParams) {
    throw 'Not Implemented.';
  }

  async UpdateEntity(entity) {
    this.ADAPTER_OUT(entity);
    return await Request('PUT', this.EndPoint, entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  // LOCAL OPERATIONS
  getById(id) {
    for (let i = 0; i < this.arrAllRecords.length; i++) {
      if (id == this.arrAllRecords[i].Id) {
        return 0;
      }
      return null;
    }
  }

  getAll() {
    for (let i = 0; i < this.arrAllRecords.length; i++) {
      this.arrAllRecords[i] = this.arrAllRecords[i];
    }
    return this.arrAllRecords;
  }

  getRecursiveBySeedId() {}

  getRawAll() {
    return this.arrAllRecords;
  }

  setRawAll(arr) {
    this.arrAllRecords = arr;
  }

  populateCatalogValues(entity) {
    for (let catalog in this.catalogs) {
      if (this.catalogs.hasOwnProperty(catalog)) {
        entity['' + catalog] = this.catalogs[catalog].getById(entity['' + catalog + 'Key']);
      }
    }
  }

  UseCommonResponse = (response, returnCommonResponse) => {
    //Make sure it is a commonResponse:
    if (!response || !response.hasOwnProperty('ErrorThrown')) throw response;

    //Check for Error:
    if (response.ErrorThrown) throw response;

    //Call Adapter In Hook:
    if (Array.isArray(response.Result)) {
      response.Result.forEach(entity => this.ADAPTER_IN(entity));
    } else if (typeof response.Result === 'object') {
      this.ADAPTER_IN(response.Result);
    }

    //Return complete CommonResponse:
    if (returnCommonResponse) return response;

    //Return only data:
    return response.Result;
  };

  UseNudeList = (response, boolWantCommonResponse) => {
    if (response.ErrorThrown) {
      throw response;
    }

    //Call Adapter In Hook:
    if (Array.isArray(response.Result)) {
      response.Result.forEach(entity => this.ADAPTER_IN(entity));
    } else if (typeof response.Result === 'object') {
      this.ADAPTER_IN(response.Result);
    }

    //Return complete CommonResponse:
    if (boolWantCommonResponse) return response;

    //Return only data:
    return response.Result;
  };

  UseNudeResponse = response => {
    //Call Adapter In Hook:
    if (Array.isArray(response.Result)) {
      response.forEach(entity => this.ADAPTER_IN(entity));
    } else if (typeof response === 'object') {
      this.ADAPTER_IN(response);
    }

    //Return only data:
    return response;
  };

  GeneralError = response => {
    //CommonResponse wrapper
    if (response.ErrorThrown) {
      switch (response.ErrorType) {
        case 'MESSAGE':
          alertify.alert(response.ResponseDescription);
      }
      return Promise.resolve();
    }
    //ServiceStack wrapper
    else if (response.ResponseStatus) {
      switch (response.ResponseStatus.ErrorCode) {
        case 'KnownError':
        case 'SqlException':
        default:
          console.log(response);
          console.log(response.ResponseStatus.StackTrace);
          alert(response.ResponseStatus.Message);
      }
    }
    //Other
    else {
      switch (response.status) {
        case 401:
          AuthService.OpenLogin();
          return Promise.reject('Your session has expired. Log in again');
      }
    }
    return Promise.reject(response.statusText);
  };

  //Formatters:===================================================================
  formatDate = (date, format = 'M/D/YYYY') => {
    if (date) return moment(date).format(format);
  };

  formatDateMD = (date, format = 'MMMM Do, YYYY') => {
    if (date) return moment(date).format(format);
  };

  formatDateLG = (date, format = 'dddd, MMMM Do, YYYY') => {
    if (date) return moment(date).format(format);
  };

  formatTime = (time, format = 'H:mm a') => {
    if (time) return moment(time).format(format);
  };

  toServerDate = date => {
    var momentDate = moment(date);
    if (momentDate.isValid()) {
      momentDate.local();
      return momentDate.format();
    }
    return null;
  };

  formatCurrency = (number, decimals = 2) => {
    if (!isNaN(number) && number > 0) {
      return new Intl.NumberFormat('en-IN', { maximumFractionDigits: decimals }).format(number);
    }
    return '';
  };

  //Hooks:=======================================================================
  ADAPTER_IN(entity) {}

  ADAPTER_OUT(entity) {}

  //Universal Catalogs:==========================================================
  async GetUniversalCatalog(catalog, params = '') {
    return await Request('GET', `catalog?name=${catalog}&${params}'&noCache=` + Number(new Date()), null, AppConfig.UniversalCatalogsURL)
      .then(e => e.Result)
      .catch(this.GeneralError);
  }

  async GetUniversalCatalogPaged(catalog, limit, page, params = '') {
    return await Request(
      'GET',
      `catalog/${limit}/${page}?name=${catalog}&${params}&noCache=` + Number(new Date()),
      null,
      AppConfig.UniversalCatalogsURL
    )
      .then(r => r.Result)
      .catch(this.GeneralError);
  }

  //Accounts:===================================================================
  async GetAccounts(params = '') {
    return await Request('GET', 'Account?' + params + '&noCache=' + Number(new Date()), null, AppConfig.AuthURL).catch(this.GeneralError);
  }
}
