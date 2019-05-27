import React from 'react';
import ListContainer from '../../core/ListContainer';
import ItemService from './item.service';

///start:slot:dependencies<<<///end:slot:dependencies<<<

const service = new ItemService();
const defaultConfig = {
  service
  ///start:slot:config<<<///end:slot:config<<<
};

class ItemsListContainer extends ListContainer {
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

  ON_OPEN_ITEM = item => {
    console.log('ON_OPEN_ITEM', item);
    ///start:slot:onOpenItem<<<
    console.log(item);
    this.openItemDialog(item);
    ///end:slot:onOpenItem<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);
    ///start:slot:afterCreate<<<
    this.openItemDialog(instance);
    ///end:slot:afterCreate<<<
  };

  openItemDialog = item => {
    this.setState({
      itemDialog: item
    });
  };

  closeItemDialog = () => {
    this.setState({
      itemDialog: false
    });
  };

  onOkItemDialog = () => {
    console.log('on ok');
    this.closeItemDialog();
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_REMOVE = () => {
    console.log('AFTER_REMOVE');
    ///start:slot:afterRemove<<<///end:slot:afterRemove<<<
  };

  ///start:slot:js<<<
  ///end:slot:js<<<

  render() {
    return <></>;
  }
}

export default ItemsListContainer;
