import { CRUDFactory } from '../../core/CRUDFactory';
// import UtilsService from '../../core/UtilsService';

///start:slot:dependencies<<<///end:slot:dependencies<<<

// const utilsService = new UtilsService();

export default class CatalogTypeService extends CRUDFactory {
  constructor() {
    super({
      EndPoint: 'CatalogType'
    });
  }
  ADAPTER_IN(entity) {
    ///start:slot:adapterIn<<<
    entity.ConvertedFields = entity.Fields ? JSON.parse(entity.Fields) : [];
    ///end:slot:adapterIn<<<
    return entity;
  }

  ADAPTER_OUT(entity) {
    ///start:slot:adapterOut<<<
    if (entity.ConvertedFields) {
      entity.ConvertedFields = entity.ConvertedFields.filter(d => {
        return Object.getOwnPropertyNames(d).some(prop => {
          if (prop == 'edited') return false;
          return d[prop];
        });
      });
      entity.Fields = JSON.stringify(entity.ConvertedFields);
    }
    ///end:slot:adapterOut<<<
  }

  ///start:slot:service<<<///end:slot:service<<<
}
