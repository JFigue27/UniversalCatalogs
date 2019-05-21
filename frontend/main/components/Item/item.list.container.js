import React from 'react';
import ListContainer from '../../core/ListContainer';
import ItemService from './item.service';

///start:slot:dependencies<<<///end:slot:dependencies<<<

const service = new ItemService();
const defaultConfig = {
  service,
  limit: 20
};

class ItemsListContainer extends ListContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
  }

  componentDidMount() {
    this.load();
  }

  render() {
    return <div />;
  }
}

export default ItemsListContainer;
