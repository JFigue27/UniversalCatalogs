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
      sortName: 'mySort',
      autoAdd: false
    },
    baseList: [],
    isLoading: true,
    filterStorageKey: 'myFilter',
    sortStorageKey: 'mySort'
  };

  constructor(props, config) {
    super(props);
    if (config) Object.assign(this.state.config, config);
    this.service = this.state.config.service;

    this.debouncedRefresh = debounce(this.refresh, 300);
    AuthService.ON_LOGIN = this.refresh;

    this.filterOptions = {
      page: 1,
      limit: config.limit
    };
    this.sortOptions = {};
  }

  bindFilterInput = event => {
    this.filterOptions[event.target.name] = event.target.value;
    this.debouncedRefresh();
  };

  componentWillUnmount() {
    if (this.debouncedRefresh && this.debouncedRefresh.cancel) this.debouncedRefresh.cancel();
  }

  // Filtering / Sorting and Local Storage:=======================================
  clearFilters = () => {
    let filterOptions = this.filterOptions || {};
    filterOptions.limit = this.state.config.limit;
    filterOptions.page = 1;
    filterOptions.itemsCount = 0;

    this.persistFilter(filterOptions);
  };

  clearSorts = () => {
    let sortOptions = {};
    this.setState({ sortOptions });
    this.persistSort(sortOptions);
  };

  persistFilter = (filters = this.filterOptions) => {
    localStorage.setItem(this.state.filterStorageKey, JSON.stringify(filters));
  };

  persistSort = (sorts = this.sortOptions) => {
    localStorage.setItem(this.state.sortStorageKey, JSON.stringify(sorts));
  };

  initFilterOptions = () => {
    let filterOptions = localStorage.getItem(this.state.filterStorageKey);

    if (!filterOptions) {
      this.clearFilters();
    } else {
      this.filterOptions = JSON.parse(filterOptions);
    }
  };

  initSortOptions = () => {
    let sortOptions = localStorage.getItem(this.state.sortStorageKey);

    if (!sortOptions) {
      this.clearSorts();
    } else {
      this.sortOptions = JSON.parse(sortOptions);
    }
  };

  // Service Operations:==========================================================
  load = async staticQueryParams => {
    this.staticQueryParams = staticQueryParams;
    // alertify.closeAll();
    this.initFilterOptions();
    this.initSortOptions();
    return await this.updateList();
  };

  updateList = async () => {
    this.setState({ isLoading: true });

    let filterOptions = { ...this.filterOptions };

    if (!this.state.config.paginate) {
      filterOptions.limit = 0;
      filterOptions.page = 1;
    }

    let page = filterOptions.page;
    let limit = filterOptions.limit;
    let query = this.makeQueryParameters(filterOptions);

    let loadData;
    if (this.customLoad) {
      loadData = this.customLoad(limit, page, query);
    } else {
      loadData = this.service.GetPaged(limit, page, query);
    }

    return await loadData
      .then(response => {
        let baseList = response.Result;

        if (this.state.config.autoAdd) baseList.push({});

        filterOptions.itemsCount = response.AdditionalData.total_filtered_items;
        filterOptions.totalItems = response.AdditionalData.total_items;

        this.persistFilter(filterOptions);
        this.persistSort();

        //Index List:
        for (let i = 0; i < baseList.length; i++) {
          let element = baseList[i];
          element.itemIndex = (filterOptions.page - 1) * filterOptions.limit + i + 1;
        }

        this.AFTER_LOAD(baseList);

        this.setState({
          baseList,
          filterOptions,
          isLoading: false
        });
      })
      .catch(e => {
        console.log(e);
        this.setState({ isLoading: false, baseList: [] });
      });
  };

  makeQueryParameters = (filterOptions = this.filterOptions, sortOptions = this.sortOptions) => {
    let result = '?';
    Object.getOwnPropertyNames(filterOptions).forEach(prop => {
      result += prop + '=' + filterOptions[prop] + '&';
    });
    Object.getOwnPropertyNames(sortOptions).forEach(prop => {
      result += 'sort-' + prop + '=' + sortOptions[prop] + '&';
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
    if (!this.filterOptions || this.filterOptions.limit == undefined) {
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
    this.filterOptions.page = newPage;
    // this.setState({
    //   filterOptions: {
    //     page: newPage
    //   }
    // });
    if (limit > 0) {
      this.filterOptions.limit = limit;
      // this.setState({
      //   filterOptions: {
      //     limit: limit
      //   }
      // });
    }
    this.updateList();
  };

  handleDateChange = (date, field, currentIndex, arrRows = this.state.baseList) => {
    arrRows[currentIndex][field] = date ? date.toDate() : null;
    arrRows[currentIndex].Entry_State = 1;
    this.onInputChange();
  };

  handleInputChange = (event, field, currentIndex, arrRows = this.state.baseList) => {
    arrRows[currentIndex][field] = event.target.value;
    arrRows[currentIndex].Entry_State = 1;
    this.onInputChange();
  };

  handleAutocompleteChange = (value, field, currentIndex, arrRows = this.state.baseList) => {
    arrRows[currentIndex][field] = value.label;
    arrRows[currentIndex].Entry_State = 1;
    this.onInputChange();
  };

  onInputChange = (arrRows = this.state.baseList) => {
    let atLeastOneFilled = false;
    if (this.state.config.autoAdd) {
      if (arrRows.length > 0) {
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
      }
    }

    this.ON_CHANGE(arrRows);
    if (atLeastOneFilled) arrRows.push({});

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
