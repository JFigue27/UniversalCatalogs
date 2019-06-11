import React from 'react';
import ListContainer from '../../core/ListContainer';

import CustomerService from './customer.service';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const service = new CustomerService();
const defaultConfig = {
  service
  ///start:slot:config<<<///end:slot:config<<<
};

class CustomersListContainer extends ListContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
  }

  componentDidMount() {
    this.load();
  }

  AFTER_LOAD = () => {
    console.log('AFTER_LOAD');
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);
    ///start:slot:afterCreate<<<
    this.openDialog(instance);
    ///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_REMOVE = () => {
    console.log('AFTER_REMOVE');
    ///start:slot:afterRemove<<<///end:slot:afterRemove<<<
  };

  ON_OPEN_ITEM = item => {
    console.log('ON_OPEN_ITEM', item);
    ///start:slot:onOpenItem<<<
    this.openDialog(item);
    ///end:slot:onOpenItem<<<
  };

  openDialog = item => {
    this.setState({
      customerDialog: item
    });
  };

  closeDialog = feedback => {
    if (feedback == 'ok') {
      this.refresh();
    }
    this.setState({
      customerDialog: false
    });
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    return <></>;
  }
}

export default CustomersListContainer;
