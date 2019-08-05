import { CRUDFactory } from '../../core/CRUDFactory';
// import UtilsService from '../../core/UtilsService';

///start:slot:dependencies<<<///end:slot:dependencies<<<

// const utilsService = new UtilsService();

export default class CatalogService extends CRUDFactory {
  constructor() {
    super({
      EndPoint: 'Catalog'
    });
  }
  ADAPTER_IN(entity) {
    ///start:slot:adapterIn<<<
    entity.ConvertedMeta = entity.Meta ? JSON.parse(entity.Meta) : {};
    ///end:slot:adapterIn<<<
    return entity;
  }

  ADAPTER_OUT(entity) {
    ///start:slot:adapterOut<<<
    // if (entity.ConvertedMeta) {
    //   entity.ConvertedMeta = entity.ConvertedMeta.filter(d => {
    //     return Object.getOwnPropertyNames(d).some(prop => {
    //       if (prop == 'edited') return false;
    //       return d[prop];
    //     });
    //   });
    // }
    entity.Meta = JSON.stringify(entity.ConvertedMeta);
    ///end:slot:adapterOut<<<
  }

  ///start:slot:service<<<///end:slot:service<<<
}
