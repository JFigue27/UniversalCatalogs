import React from 'react';
import FormContainer from '../../core/FormContainer';

import MaterialService from './material.service';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const service = new MaterialService();
const defaultConfig = {
  service
  ///start:slot:config<<<///end:slot:config<<<
};

class MaterialFormContainer extends FormContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
  }

  AFTER_LOAD = entity => {
    console.log('AFTER_LOAD', entity);
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);

    ///start:slot:afterCreate<<<///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_SAVE = entity => {
    console.log('AFTER_SAVE', entity);
    ///start:slot:afterSave<<<///end:slot:afterSave<<<
  };

  BEFORE_CHECKIN = () => {
    console.log('BEFORE_CHECKIN');
    ///start:slot:beforeCheckin<<<///end:slot:beforeCheckin<<<
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    return <></>;
  }
}

export default MaterialFormContainer;
