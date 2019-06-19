import { CRUDFactory } from '../../core/CRUDFactory';
// import UtilsService from '../../core/UtilsService';

///start:slot:dependencies<<<///end:slot:dependencies<<<

// const utilsService = new UtilsService();

export default class CustomerService extends CRUDFactory {
  constructor() {
    super({
      EndPoint: 'Customer'
    });
  }
  ADAPTER_IN(entity) {
    ///start:slot:adapterIn<<<///end:slot:adapterIn<<<
    return entity;
  }

  ADAPTER_OUT(entity) {
    ///start:slot:adapterOut<<<///end:slot:adapterOut<<<
  }

  ///start:slot:service<<<///end:slot:service<<<
}