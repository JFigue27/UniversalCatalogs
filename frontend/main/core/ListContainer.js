import React from 'react';
import _ from 'lodash';
import AuthService from './AuthService';

class ListContainer extends React.Component {
  state = {
    config: {
      service: null,
      paginate: true,
      limit: 10,
      filters: '',
      filterName: 'myFilter',
      sortName: 'mySort'
    },
    baseList: [],
    isLoading: false,
    filterStorageKey: 'myFilter',
    sortStorageKey: 'mySort',
    filterOptions: {
      page: 1,
      limit: 10
    },
    sortOptions: {},
    staticQueryParams: ''
  };

  constructor(props, config) {
    super(props);
    if (config) Object.assign(this.state.config, config);
    this.service = this.state.config.service;

    this.debouncedRefresh = _.debounce(this.refresh, 300);
    AuthService.ON_LOGIN = this.refresh;
  }

  bindFilterInput = event => {
    this.state.filterOptions[event.target.name] = event.target.value;
    this.debouncedRefresh();
  };

  componentWillUnmount() {
    this.debouncedRefresh.cancel();
  }

  // Filtering / Sorting and Local Storage:=======================================
  clearFilters = () => {
    this.setState({
      filterOptions: {
        limit: this.state.config.limit,
        page: 1,
        itemsCount: 0
      }
    });
    this.persistFilter();
  };

  clearSorts = () => {
    this.setState({ sortOptions: {} });
    this.persistSort();
  };

  persistFilter = () => {
    localStorage.setItem(this.state.filterStorageKey, JSON.stringify(this.state.filterOptions));
  };

  persistSort = () => {
    localStorage.setItem(this.state.sortStorageKey, JSON.stringify(this.state.sortOptions));
  };

  setFilterOptions = () => {
    let filterOptions = localStorage.getItem(this.state.filterStorageKey);

    if (!filterOptions) {
      this.clearFilters();
    } else {
      filterOptions = this.state.filterOptions;
      this.setState({ filterOptions });
    }
  };

  setSortOptions = () => {
    let sortOptions = localStorage.getItem(this.state.sortStorageKey);

    if (!sortOptions) {
      this.clearSorts();
    } else {
      sortOptions = this.state.sortOptions;
      this.setState({ sortOptions });
    }
  };

  // Service Operations:==========================================================
  load = async staticQueryParams => {
    this.state.staticQueryParams = staticQueryParams;
    // alertify.closeAll();
    this.setFilterOptions();
    this.setSortOptions();
    return await this.updateList();
  };

  updateList = async () => {
    this.setState({ isLoading: true });

    if (!this.state.config.paginate) {
      this.setState({
        filterOptions: {
          limit: 0,
          page: 1
        }
      });
    }

    let page = this.state.filterOptions.page;
    let limit = this.state.filterOptions.limit;
    let queryParameters = this.makeQueryParameters();

    return await this.service
      .GetPaged(limit, page, queryParameters)
      .then(response => {
        this.setState({
          baseList: response.Result,
          filterOptions: {
            itemsCount: response.AdditionalData.total_filtered_items,
            totalItems: response.AdditionalData.total_items,
            page,
            limit
          }
        });

        this.persistFilter();
        this.persistSort();

        //Index List:
        for (let i = 0; i < this.state.baseList.length; i++) {
          let element = this.state.baseList[i];
          element.itemIndex = (this.state.filterOptions.page - 1) * this.state.filterOptions.limit + i + 1;
        }

        this.AFTER_LOAD();
        this.setState({ isLoading: false });
      })
      .catch(e => {
        console.log(e);
        this.setState({ isLoading: false, baseList: [] });
      });
  };

  makeQueryParameters = () => {
    let result = '?';
    Object.getOwnPropertyNames(this.state.filterOptions).forEach(prop => {
      result += prop + '=' + this.state.filterOptions[prop] + '&';
    });
    Object.getOwnPropertyNames(this.state.sortOptions).forEach(prop => {
      result += 'sort-' + prop + '=' + this.state.sortOptions[prop] + '&';
    });
    result += this.state.staticQueryParams || '';
    return result;
  };

  refresh = () => {
    if (!this.state.filterOptions || this.state.filterOptions.limit == undefined) {
      this.clearFilters();
    } else {
      this.updateList();
    }
  };

  createInstance = (event, item) => {
    // let theArguments = Array.prototype.slice.call(arguments);
    this.service.CreateInstance(item).then(oInstance => {
      // theArguments.unshift(oInstance);
      // this.AFTER_CREATE.apply(this, theArguments);
      this.AFTER_CREATE(oInstance);
    });
  };

  saveItem = item => {
    return this.service.Save(item).then(entity => {
      // alertify.success('SUCCESFULLY SAVED');
      return Promise.resolve(entity);
    });
  };

  removeItem = (event, item) => {
    if (event) event.stopPropagation();
    if (confirm('Do you really want to delete it?')) console.log(item);
  };

  removeSelected = () => {
    throw 'Not Implemented';
  };

  save = () => {
    throw 'Not Implemented';
  };

  createAndCheckout = async (event, item = {}) => {
    if (event) event.stopPropagation();
    if (confirm(`Please confirm to create a new ${this.service.config.EndPoint}`)) {
      return await this.service.CreateAndCheckout(item).then(entity => {
        this.AFTER_CREATE_AND_CHECKOUT(entity);
        console.log('success');
        return entity;
      });
    }
  };

  // Local Operations:============================================================
  undoItem = () => {
    throw 'Not Implemented';
  };

  selectAll = () => {
    throw 'Not Implemented';
  };

  unSelectAll = () => {
    throw 'Not Implemented';
  };

  checkItem = () => {
    throw 'Not Implemented';
  };

  getSelected = () => {
    throw 'Not Implemented';
  };

  getSelectedCount = () => {
    throw 'Not Implemented';
  };

  clear = () => {
    this.setState({
      baseList: []
    });
  };

  // Events:======================================================================
  openItem = entity => {
    // var theArguments = Array.prototype.slice.call(arguments);
    // this.ON_OPEN_ITEM.apply(this, theArguments);
    this.ON_OPEN_ITEM(entity);
  };

  pageChanged = (newPage, limit) => {
    this.setState({
      filterOptions: {
        page: newPage
      }
    });
    if (limit > 0) {
      this.setState({
        filterOptions: {
          limit: limit
        }
      });
    }
    this.updateList();
  };

  on_input_change = oItem => {
    oItem.editMode = true;
  };

  // Hooks:=======================================================================
  AFTER_LOAD = () => {};

  ON_OPEN_ITEM = entity => {};

  AFTER_REMOVE = () => {};

  AFTER_CREATE = () => {};

  AFTER_CREATE_AND_CHECKOUT = entity => {};

  render() {
    return <div />;
  }
}

export default ListContainer;
