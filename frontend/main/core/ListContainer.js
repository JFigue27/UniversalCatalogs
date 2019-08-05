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

const storageSufixx = 'UniversalCatalogs';

class ListContainer extends FormContainer {
  state = {
    service: null,
    paginate: true,
    limit: 10,
    filters: '',
    filterName: 'myFilter',
    sortName: 'mySort',
    autoAdd: false,
    baseList: [],
    isLoading: true,
    isAllSelected: false,
    isAllUnselected: true,
    filterOptions: {},
    sortOptions: {}
  };

  constructor(props, config) {
    super(props);
    if (config) Object.assign(this.state, config);
    this.service = this.state.service;

    this.debouncedRefresh = debounce(this.refresh, 250);
    AuthService.ON_LOGIN = this.refresh;
  }

  bindFilterInput = event => {
    const { filterOptions } = this.state;
    filterOptions[event.target.name] = event.target.value;
    this.setState({ filterOptions });
    this.persistFilter(filterOptions);
    this.debouncedRefresh();
  };

  bindFilterInputNoRefresh = event => {
    const { filterOptions } = this.state;
    filterOptions[event.target.name] = event.target.value;
    this.persistFilter(filterOptions);
    this.setState({ filterOptions });
  };

  generalSearchOnEnter = event => {
    if (event.charCode == 13) this.refresh();
  };

  componentWillMount() {
    this.initFilterOptions();
    this.initSortOptions();
  }

  componentWillUnmount() {
    if (this.debouncedRefresh && this.debouncedRefresh.cancel) this.debouncedRefresh.cancel();
  }

  // Filtering / Sorting and Local Storage:=======================================
  clearFilters = () => {
    const { filterOptions } = this.state;
    filterOptions.limit = this.state.limit;
    filterOptions.page = 1;
    filterOptions.itemsCount = 0;

    this.setState({ filterOptions });
    this.persistFilter(filterOptions);
  };

  clearSorts = () => {
    const { sortOptions } = this.state;

    this.setState({ sortOptions });
    this.persistSort(sortOptions);
  };

  persistFilter = (filters = this.state.filterOptions) => {
    if (process.browser) localStorage.setItem(storageSufixx + '.f.' + this.state.filterName, JSON.stringify(filters));
  };

  persistSort = (sorts = this.state.sortOptions) => {
    if (process.browser) localStorage.setItem(storageSufixx + '.s.' + this.state.sortName, JSON.stringify(sorts));
  };

  initFilterOptions = (filterName = this.state.filterName) => {
    this.state.filterName = filterName;
    let filterOptions = process.browser && localStorage.getItem(storageSufixx + '.f.' + filterName);

    if (!filterOptions) {
      this.clearFilters();
    } else {
      filterOptions = JSON.parse(filterOptions);
      this.setState({ filterOptions });
    }
  };

  initSortOptions = (sortName = this.state.sortName) => {
    this.state.sortName = sortName;
    let sortOptions = process.browser && localStorage.getItem(storageSufixx + '.s.' + sortName);

    if (!sortOptions) {
      this.clearSorts();
    } else {
      sortOptions = JSON.parse(sortOptions);
      this.setState({ sortOptions });
    }
  };

  // Service Operations:==========================================================
  load = async staticQueryParams => {
    this.staticQueryParams = staticQueryParams;
    // alertify.closeAll();
    // this.initFilterOptions();
    // this.initSortOptions();
    return await this.updateList();
  };

  updateList = async () => {
    this.setState({ isLoading: true });

    let { filterOptions } = this.state;

    if (!this.state.paginate) {
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

        filterOptions.itemsCount = response.AdditionalData.total_filtered_items;
        filterOptions.totalItems = response.AdditionalData.total_items;
        filterOptions.page = response.AdditionalData.page || page;

        this.persistFilter(filterOptions);
        this.persistSort();

        //Index List:
        for (let i = 0; i < baseList.length; i++) {
          let element = baseList[i];
          element.itemIndex = (filterOptions.page - 1) * filterOptions.limit + i + 1;
        }

        this.AFTER_LOAD(baseList);

        this.ON_CHANGE(baseList);

        if (this.state.autoAdd) baseList.push({});

        this.setState({
          baseList,
          isLoading: false,
          filterOptions
        });
      })
      .catch(e => {
        console.log(e);
        this.ON_CHANGE([]);
        this.setState({ isLoading: false, baseList: [] });
      });
  };

  makeQueryParameters = (filterOptions = this.state.filterOptions, sortOptions = this.state.sortOptions) => {
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
    if (this.state.filterOptions.limit == undefined) {
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
    if (confirm('Do you really want to delete it?')) {
      this.service.RemoveById(item.Id).then(() => {
        this.AFTER_REMOVE(item);
      });
    }
  };

  localRemoveItem = (event, index, arrRows = this.state.baseList) => {
    if (event) event.stopPropagation();
    arrRows.splice(index, 1);
    this.onInputChange();
  };

  localAddItem = (arrRows = this.state.baseList) => {
    arrRows.push({});
    this.onInputChange();
  };

  removeSelected = () => {
    throw 'Not Implemented';
  };

  save = () => {
    throw 'Not Implemented';
  };

  createAndCheckout = async (event, item = {}) => {
    if (event) event.stopPropagation();
    if (confirm(`Please confirm to create a new ${this.service.EndPoint}`)) {
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
    const { baseList } = this.state;
    for (let item of baseList) {
      item.selected = true;
    }
    this.setState({ baseList, isAllSelected: true, isAllUnselected: false });
    this.ON_CHANGE(baseList);
  };

  unselectAll = () => {
    const { baseList } = this.state;
    for (let item of baseList) {
      item.selected = false;
    }
    this.setState({ baseList, isAllSelected: false, isAllUnselected: true });
    this.ON_CHANGE(baseList);
  };

  toggleSelect = index => {
    const { baseList } = this.state;
    baseList[index].selected = !baseList[index].selected;
    this.setState({ baseList, isAllSelected: false, isAllUnselected: false });
    this.ON_CHANGE(baseList);
  };

  getSelected = () => {
    throw 'Not Implemented';
  };

  getSelectedCount = () => {
    throw 'Not Implemented';
  };

  clear = () => {
    this.ON_CHANGE([]);
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
    const { filterOptions } = this.state;
    filterOptions.page = newPage;
    if (limit > 0) filterOptions.limit = limit;
    this.setState({ filterOptions });
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

  handleCheckBoxChange = (event, field, currentIndex, arrRows = this.state.baseList) => {
    arrRows[currentIndex][field] = event.target.checked;
    arrRows[currentIndex].Entry_State = 1;
    this.onInputChange();
  };

  handleToggleListItem = (field, currentIndex, arrRows = this.state.baseList) => {
    arrRows[currentIndex][field] = !arrRows[currentIndex][field];
    arrRows[currentIndex].Entry_State = 1;
    this.onInputChange();
  };

  onInputChange = (arrRows = this.state.baseList) => {
    let atLeastOneFilled = false;
    if (this.state.autoAdd) {
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
        this.find('input,textarea,button').keydown(function(e) {
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
              moveTo = td.prev('td:has(input,textarea,button)');
              // }
              break;
            }
            case arrow.right: {
              // if (input.selectionEnd == input.value.length) {
              moveTo = td.next('td:has(input,textarea,button)');
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
            moveTo.find('input,textarea,button').each(function(i, input) {
              input.focus();
              if (input.type != 'button') {
                input.select();
              }
            });
          }
        });
      };
    })(jQuery);

    jQuery(table).enableCellNavigation();
  };

  // Hooks:=======================================================================
  AFTER_LOAD = baseList => {};

  ON_OPEN_ITEM = entity => {};

  AFTER_CREATE = instance => {};

  AFTER_CREATE_AND_CHECKOUT = entity => {};

  AFTER_REMOVE = () => {};

  render() {
    return <div />;
  }
}

export default ListContainer;
