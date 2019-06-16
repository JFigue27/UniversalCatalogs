import React from 'react';
import FormContainer from './FormContainer';
import AuthService from './AuthService';
import jQuery from 'jquery';

function debounce(func, wait, immediate) {
  let timeout;
  return function() {
    let context = this,
      args = arguments;
    let later = function() {
      timeout = null;
      if (!immediate) func.apply(context, args);
    };
    let callNow = immediate && !timeout;
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
    if (callNow) func.apply(context, args);
  };
}

class ListContainer extends FormContainer {
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
    isLoading: true,
    filterStorageKey: 'myFilter',
    sortStorageKey: 'mySort',
    filterOptions: {
      page: 1,
      limit: 10
    },
    sortOptions: {}
  };

  constructor(props, config) {
    super(props);
    if (config) Object.assign(this.state.config, config);
    this.service = this.state.config.service;

    this.debouncedRefresh = debounce(this.refresh, 300);
    AuthService.ON_LOGIN = this.refresh;
  }

  bindFilterInput = event => {
    this.state.filterOptions[event.target.name] = event.target.value;
    this.debouncedRefresh();
  };

  componentWillUnmount() {
    if (this.debouncedRefresh.cancel) this.debouncedRefresh.cancel();
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
    this.staticQueryParams = staticQueryParams;
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
    let query = this.makeQueryParameters();

    let loadData;
    if (this.customLoad) {
      loadData = this.customLoad(limit, page, query);
    } else {
      loadData = this.service.GetPaged(limit, page, query);
    }

    return await loadData
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

    if (this.staticQueryParams)
      if (this.staticQueryParams instanceof Object || typeof this.staticQueryParams == 'object') {
        Object.getOwnPropertyNames(this.staticQueryParams).forEach(prop => {
          result += `&${prop}=${this.staticQueryParams[prop]}`;
        });
      } else {
        result += this.staticQueryParams || '';
      }

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
  openItem = (event, item) => {
    // let theArguments = Array.prototype.slice.call(arguments);
    // this.ON_OPEN_ITEM.apply(this, theArguments);
    this.ON_OPEN_ITEM(item);
  };

  pageChanged = (newPage, limit) => {
    this.state.filterOptions.page = newPage;
    // this.setState({
    //   filterOptions: {
    //     page: newPage
    //   }
    // });
    if (limit > 0) {
      this.state.filterOptions.limit = limit;
      // this.setState({
      //   filterOptions: {
      //     limit: limit
      //   }
      // });
    }
    this.updateList();
  };

  onInputChange = (event, field, currentIndex, arrRows = this.state.baseList) => {
    arrRows[currentIndex][field] = event.target.value;
    arrRows[currentIndex].edited = true;

    if (arrRows.length > 0) {
      let atLeastOneFilled = false;
      let lastRow = arrRows[arrRows.length - 1];
      for (let prop in lastRow) {
        if (lastRow.hasOwnProperty(prop)) {
          if (prop == 'Id') {
            continue;
          }
          if (lastRow[prop] && (lastRow[prop] > 0 || lastRow[prop].length > 0)) {
            atLeastOneFilled = true;
            break;
          }
        }
      }
      if (atLeastOneFilled) arrRows.push({});
    }

    this.setState({
      baseList: arrRows
    });
  };

  // Utils:=======================================================================
  enableCellNavigation = table => {
    (function($) {
      $.fn.enableCellNavigation = function() {
        let arrow = {
          left: 37,
          up: 38,
          right: 39,
          down: 40,
          enter: 13
        };

        // select all on focus
        // works for input elements, and will put focus into
        // adjacent input or textarea. once in a textarea,
        // however, it will not attempt to break out because
        // that just seems too messy imho.
        this.find('input').keydown(function(e) {
          // shortcut for key other than arrow keys
          if ($.inArray(e.which, [arrow.left, arrow.up, arrow.right, arrow.down, arrow.enter]) < 0) {
            return;
          }

          e.preventDefault();

          let input = e.target;
          let td = $(e.target).closest('td');
          let moveTo = null;

          switch (e.which) {
            case arrow.left: {
              // if (input.selectionStart == 0) {
              moveTo = td.prev('td:has(input,textarea)');
              // }
              break;
            }
            case arrow.right: {
              // if (input.selectionEnd == input.value.length) {
              moveTo = td.next('td:has(input,textarea)');
              // }
              break;
            }

            case arrow.up:
            case arrow.enter:
            case arrow.down: {
              let tr = td.closest('tr');
              let pos = td[0].cellIndex;

              let moveToRow = null;
              if (e.which == arrow.down || e.which == arrow.enter) {
                moveToRow = tr.next('tr');
              } else if (e.which == arrow.up) {
                moveToRow = tr.prev('tr');
              }

              if (moveToRow.length) {
                moveTo = $(moveToRow[0].cells[pos]);
              }
              break;
            }
          }

          if (moveTo && moveTo.length) {
            moveTo.find('input,textarea').each(function(i, input) {
              input.focus();
              input.select();
            });
          }
        });
      };
    })(jQuery);

    jQuery(table).enableCellNavigation();
  };

  // Hooks:=======================================================================
  AFTER_LOAD = () => {};

  ON_OPEN_ITEM = entity => {};

  AFTER_CREATE = instance => {};

  AFTER_CREATE_AND_CHECKOUT = entity => {};

  AFTER_REMOVE = () => {};

  render() {
    return <div />;
  }
}

export default ListContainer;
