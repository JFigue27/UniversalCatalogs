import 'isomorphic-fetch';
import moment from 'moment';
import AppConfig from './AppConfig';
import AuthService from '../core/AuthService';

const GeneralError = response => {
  //CommonResponse wrapper
  if (response.ErrorThrown) {
    switch (response.ErrorType) {
      case 'MESSAGE':
        alertify.alert(response.ResponseDescription);
    }
    throw response.ResponseDescription;
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
    throw response.ResponseStatus.Message;
  }
  //Other
  else {
    switch (response.status) {
      case 401:
        AuthService.OpenLogin();
        throw 'Your session has expired. Log in again';
    }
  }
  throw response;
};

const Request = async (method, endpoint, data, BaseURL) => {
  if (AuthService.auth == null) AuthService.fillAuthData();
  if (!AuthService.auth || !AuthService.auth.user) throw 'User not signed in.';

  const config = {
    method: method,
    mode: 'cors',
    // cache: 'no-cache',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${AuthService.auth.user.BearerToken}`
    }
  };
  if (['POST', 'PUT', 'DELETE'].includes(method)) config.body = JSON.stringify(data);
  let response = await fetch((BaseURL || AppConfig.BaseURL) + endpoint, config);
  if (response) {
    if (!response.ok) throw await response.json();
    if (response.status == 403) alert('Invalid Role.');
    if (response.status == 401) throw response;
  } else {
    alert('Failed to fetch. Probably server is down.');
  }
  return await response.json();
};

const Get = async (endpoint, data, baseURL) => await Request('GET', endpoint, data, baseURL).catch(GeneralError);
const Post = async (endpoint, data, baseURL) => await Request('POST', endpoint, data, baseURL).catch(GeneralError);
const Put = async (endpoint, data, baseURL) => await Request('PUT', endpoint, data, baseURL).catch(GeneralError);
const Delete = async (endpoint, data, baseURL) => await Request('DELETE', endpoint, data, baseURL).catch(GeneralError);

export class CRUDFactory {
  constructor(config) {
    this.config = config;
    this.EndPoint = config.EndPoint;
  }

  async InsertEntity(entity) {
    this.ADAPTER_OUT(entity);
    return await Post(this.EndPoint, entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async CreateAndCheckout(entity) {
    this.ADAPTER_OUT(entity);
    return await Post(this.EndPoint + '/CreateAndCheckout', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Checkout(entity) {
    return await Post(this.EndPoint + '/Checkout/' + entity.Id)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async CancelCheckout(entity) {
    return await Post(this.EndPoint + '/CancelCheckout/' + entity.Id)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Checkin(entity) {
    return await Post(this.EndPoint + '/Checkin', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Finalize(entity) {
    this.ADAPTER_OUT(entity);
    return await Post(this.EndPoint + '/Finalize', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Unfinalize(entity) {
    this.ADAPTER_OUT(entity);
    return await Post(this.EndPoint + '/Unfinalize', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async MakeRevision(entity) {
    return await Post(this.EndPoint + '/MakeRevision', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Duplicate(entity) {
    return await Post(this.EndPoint + '/Duplicate', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async CreateInstance(entity) {
    return await Post(this.EndPoint + '/CreateInstance', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async Get(operation) {
    return await Get(this.EndPoint + '/' + operation).catch(this.GeneralError);
  }

  async Post(operation, entity = {}) {
    return await Post(this.EndPoint + '/' + operation, entity).catch(this.GeneralError);
  }

  async GetPaged(limit, page, params = '?') {
    return await Get(this.EndPoint + '/getPaged/' + limit + '/' + page + params)
      .then(r => this.UseCommonResponse(r, true))
      .catch(this.GeneralError);
  }

  async GetSingleWhere(property, value, params = '') {
    if (property && value) {
      return await Get(this.EndPoint + '/GetSingleWhere/' + property + '/' + value + '?' + params)
        .then(r => this.UseNudeResponse(r))
        .catch(this.GeneralError);
    } else if (params.length > 1) {
      return await Get(this.EndPoint + '/GetSingleWhere?' + params)
        .then(r => this.UseNudeResponse(r))
        .catch(this.GeneralError);
    } else {
      return Promise.reject('Invalid params for GetSingleWhere.');
    }
  }

  async LoadEntities(params = '?') {
    return await Get(this.EndPoint + params)
      .then(r => this.UseNudeResponse(r))
      .catch(this.GeneralError);
  }

  async LoadEntity(id) {
    if (id) {
      return await Get(this.EndPoint + '/' + id)
        .then(r => this.UseNudeResponse(r))
        .catch(this.GeneralError);
    } else {
      return Promise.reject('Id not found');
    }
  }

  async Remove(entity) {
    return await Delete(this.EndPoint, entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async RemoveById(id) {
    return await Delete(this.EndPoint + '/' + id)
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
    return await Post(this.EndPoint + '/SendTestEmail', entity)
      .then(r => this.UseCommonResponse(r))
      .catch(this.GeneralError);
  }

  async SetProperty(entity, sProperty, Value, qParams) {
    throw 'Not Implemented.';
  }

  async UpdateEntity(entity) {
    this.ADAPTER_OUT(entity);
    return await Put(this.EndPoint, entity)
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
      response.Result.forEach(entity => {
        this.closeRevisions(entity);
        this.ADAPTER_IN(entity);
      });
    } else if (typeof response.Result === 'object') {
      this.closeRevisions(response.Result);
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
      response.Result.forEach(entity => {
        this.closeRevisions(entity);
        this.ADAPTER_IN(entity);
      });
    } else if (typeof response.Result === 'object') {
      this.closeRevisions(response.Result);
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
      response.forEach(entity => {
        this.closeRevisions(entity);
        this.ADAPTER_IN(entity);
      });
    } else if (typeof response === 'object') {
      this.closeRevisions(response);
      this.ADAPTER_IN(response);
    }

    //Return only data:
    return response;
  };

  closeRevisions = entity => {
    entity &&
      entity.Revisions &&
      entity.Revisions.forEach(revision => {
        revision.isOpened = false;
      });
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
    var momentDate = moment(date || null);
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
  ADAPTER_IN(entity) {
    return entity;
  }

  ADAPTER_OUT(entity) {
    return entity;
  }

  //Catalogs:====================================================================
  async GetCatalog(name, params = '', limit = 0, page = 1) {
    return await Get(`catalog/${limit}/${page}?name=${name}&${params}`)
      .then(r => r.Result)
      .catch(this.GeneralError);
  }

  async GetUniversalCatalog(name, params = '', limit = 0, page = 1) {
    return await Get(`catalog/${limit}/${page}?name=${name}&${params}`, null, AppConfig.UniversalCatalogsURL)
      .then(r => r.Result)
      .catch(this.GeneralError);
  }

  //Accounts:===================================================================
  async GetAccounts(params = '') {
    return await Get('Account?' + params, null, AppConfig.AuthURL).catch(this.GeneralError);
  }
}
