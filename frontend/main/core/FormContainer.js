import React from 'react';
import Router from 'next/router';
import { GlobalContext } from '../components/App/globals-context';

class FormContainer extends React.Component {
  state = {
    config: {
      service: null
    },
    isLoading: true,
    baseEntity: {},
    isDisabled: true
  };

  auth = {};

  constructor(props, config) {
    super(props);
    if (config) Object.assign(this.state.config, config);

    this.service = this.state.config.service;
  }

  UNSAFE_componentWillMount() {
    this.auth = this.context.auth;
  }

  componentDidUpdate() {
    if (!this.auth && this.context.auth && this.context.auth.user) {
      console.log('Refresh after login.');
      this.refreshForm();
    }
    this.auth = this.context.auth;
  }

  // Service Operations:==========================================================
  load = async criteria => {
    this.criteria = criteria;
    return await this.refreshForm();
  };

  refreshForm = async criteriaParam => {
    let criteria = criteriaParam === undefined ? this.criteria : criteriaParam;
    console.log('Form criteria: ' + criteria);

    //Clear form:
    if (criteria === null) {
      console.log('Form Reset.', criteria);
      const baseEntity = {};
      this.ON_CHANGE(baseEntity);
      this.setState({ baseEntity, isLoading: false });
    }
    //Open by ID
    else if (!isNaN(criteria) && criteria > 0) {
      console.log('Form opened by ID.', criteria);
      //TODO: Catch non-existent record
      return this.service.LoadEntity(criteria).then(baseEntity => {
        baseEntity.isOpened = true;
        this._afterLoad(baseEntity);
        this.ON_CHANGE(baseEntity);
        this.setState({
          isLoading: false,
          baseEntity
        });
      });
    }
    //Open by query parameters
    else if (typeof criteria == 'string') {
      return this.service.GetSingleWhere(null, null, criteria).then(baseEntity => {
        baseEntity.isOpened = true;
        this._afterLoad(baseEntity);
        this.ON_CHANGE(baseEntity);
        this.setState({ isLoading: false, baseEntity });
      });
    }
    //Create instance
    else if ((criteria instanceof Object || typeof criteria == 'object') && !criteria.hasOwnProperty('Id')) {
      console.log('Form opened by Create Instance.', criteria);
      return this.createInstance(criteria);
    }
    //Open direct object
    else if (criteria instanceof Object || typeof criteria == 'object') {
      console.log('Form opened by Object.', criteria);
      criteria.isOpened = true;
      this.service.ADAPTER_IN(criteria);
      this._afterLoad(criteria);
      this.ON_CHANGE(criteria);
      this.setState({
        baseEntity: criteria,
        isLoading: false
      });
    }
  };

  createInstance = async (event, predefined) => {
    // let theArguments = Array.prototype.slice.call(arguments);
    return await this.service.CreateInstance(predefined).then(instance => {
      // theArguments.unshift(instance);
      // this.AFTER_CREATE.apply(this, theArguments);
      instance.Entry_State = 1;
      this.AFTER_CREATE(instance);
      this.ON_CHANGE(instance);
      this.setState({ baseEntity: instance, isDisabled: false });
    });
  };

  createAndCheckout = async (event, item = {}) => {
    if (event) event.stopPropagation();
    if (confirm(`Please confirm to create a new ${this.service.config.EndPoint}`)) {
      return await this.service.CreateAndCheckout(item).then(baseEntity => {
        this.AFTER_CREATE_AND_CHECKOUT(baseEntity);
        this.setState({ baseEntity, isDisabled: false });
        this.success('Created and Checked Out.');
        return baseEntity;
      });
    }
  };

  save = () => {
    return this.BEFORE_SAVE(this.state.baseEntity)
      .then((entity = this.state.baseEntity) => {
        return this.service.Save(entity).then(baseEntity => {
          baseEntity.isOpened = true;
          this.AFTER_SAVE(baseEntity);
          this.ON_CHANGE(baseEntity);
          this.setState({ baseEntity });
          this.success('Saved.');
        });
      })
      .catch(error => {
        console.log(error);
        let sError = JSON.stringify(error);
        this.error(sError);
        // alert(sError);
      });
  };

  loadRevision = selectedRevision => {
    this.state.baseEntity.Revisions.forEach(revision => {
      revision.isOpened = false;
    });

    let revision = JSON.parse(selectedRevision.Value);
    revision.Revisions = this.state.baseEntity.Revisions;
    this.service.ADAPTER_IN(revision);
    selectedRevision.isOpened = true;
    this.ON_CHANGE(revision);
    this.setState({ baseEntity: revision });
    this.success('Revision Loaded.');
  };

  take = async (entity, toUser) => {
    return await this.service.Take(entity, toUser).then(() => {
      entity.assignedTo = toUser.Value;
      entity.AssignationMade = false;
      this.success('Assigned.');
    });
  };

  remove = async (event, entity = this.state.baseEntity) => {
    if (event) event.stopPropagation();
    if (confirm(`Are you sure you want to remove it?`)) {
      return await this.service.Remove(entity).then(() => {
        this.AFTER_REMOVE(entity);
        this.success('Removed.');
      });
    }
  };

  duplicate = async () => {
    return await this.service.Duplicate(this.state.baseEntity).then(() => {
      this.success('Duplicated.');
    });
  };

  checkout = async () => {
    return await this.service
      .Checkout(this.state.baseEntity)
      .then(response => {
        this.refreshForm();
        // this.setState({ isDisabled: false });
        this.success('Checked Out.');
      })
      .catch(() => {
        this.load(this.state.baseEntity.Id);
      });
  };

  cancelCheckout = async () => {
    return await this.service.CancelCheckout(this.state.baseEntity).then(response => {
      this.refreshForm();
      // this.setState({ isDisabled: true });
      this.success('Cancel Checked Out.');
    });
  };

  checkin = async event => {
    if (event) event.stopPropagation();
    let { baseEntity } = this.state;

    let message = prompt(`Optional message to reference this update.`);
    if (message !== null) {
      baseEntity.Entry_State = 1;
      baseEntity.RevisionMessage = message;
      return this.BEFORE_CHECKIN(baseEntity)
        .then((entity = baseEntity) => this.BEFORE_SAVE(entity))
        .then((entity = baseEntity) => {
          return this.service.Checkin(entity).then(response => {
            this.refreshForm(response).then(() => {
              this.formMode = null;
              // this.setState({ isDisabled: true });
              this.success('Checked In.');
            });
          });
        });
    }
  };

  finalize = async () => {
    return await this.service
      .Finalize(this.state.baseEntity)
      .then(response => {
        this.load(response);
        this.success('Finalized.');
      })
      .catch(() => {
        this.load(this.state.baseEntity.Id);
      });
  };

  unfinalize = async () => {
    return await this.service
      .Unfinalize(this.state.baseEntity)
      .then(response => {
        this.load(response);
        this.success('Unfinalized.');
      })
      .catch(() => {
        this.load(this.state.baseEntity.Id);
      });
  };

  onDialogOk = async () => await this.save();

  // Local Operations:============================================================
  handleInputChange = (event, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = event.target.value;
    this.ON_CHANGE(baseEntity, field);
    this.setState({ baseEntity });
  };

  handleCheckBoxChange = (event, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = event.target.checked;
    this.ON_CHANGE(baseEntity, field);
    this.setState({ baseEntity });
  };

  handleDateChange = (date, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = date && date.toDate ? date.toDate() : null;
    this.ON_CHANGE(baseEntity, field);
    this.setState({ baseEntity });
  };

  handleRichText = (event, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = event.value;
    this.ON_CHANGE(baseEntity, field);
    this.setState({ baseEntity });
  };

  handleChipsChange = (value, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = value;
    this.ON_CHANGE(baseEntity, field);
    this.setState({ baseEntity });
  };

  handleAutocompleteChange = (value, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = value ? value.label : null;
    this.ON_CHANGE(baseEntity, field);
    this.setState({ baseEntity });
  };

  getCurrentUser = () => (this.auth && this.auth.user) || {};

  getCheckoutUser = entity => {
    let baseEntity = entity || this.state.baseEntity;
    if (baseEntity && baseEntity.CheckedoutBy) return baseEntity.CheckedoutBy;
    return '';
  };

  isCheckedOutByCurrentUser = entity => {
    let user = this.getCurrentUser();
    let checkedOutBy = this.getCheckoutUser(entity);
    if (user.UserName && checkedOutBy && checkedOutBy.toLowerCase() == user.UserName.toLowerCase()) {
      return true;
    }
    return false;
  };

  _afterLoad = baseEntity => {
    console.log('_afrerLoad');
    if (this.isCheckedOutByCurrentUser(baseEntity)) {
      // debugger;
      this.setState({ isDisabled: false });
    } else {
      // debugger;
      this.setState({ isDisabled: true });
    }

    this.AFTER_LOAD(baseEntity);
  };

  clear = () => {
    const baseEntity = {};
    this.ON_CHANGE(baseEntity);
    this.setState({ baseEntity });
  };

  makeQueryParameters = fromObject => {
    let result = '?';
    if (fromObject instanceof Object || typeof fromObject == 'object')
      Object.getOwnPropertyNames(fromObject).forEach(prop => {
        result += `&${prop}=${fromObject[prop]}`;
      });

    return result;
  };

  navigateTo(href) {
    return Router.push(href);
  }

  success = (message, autoHideDuration = 700) => {
    this.props.enqueueSnackbar(message, { variant: 'success', autoHideDuration });
  };
  error = (message, autoHideDuration = 3000) => {
    this.props.enqueueSnackbar(message, { variant: 'error', autoHideDuration });
  };
  info = (message, autoHideDuration = 700) => {
    this.props.enqueueSnackbar(message, { variant: 'info', autoHideDuration });
  };
  message = (message, autoHideDuration = 700) => {
    this.props.enqueueSnackbar(message, { autoHideDuration });
  };

  //Formatters:===================================================================
  formatDate = (date, format) => {
    if (this.service) return this.service.formatDate(date, format);
  };
  formatDateMD = (date, format) => {
    if (this.service) return this.service.formatDateMD(date, format);
  };
  formatDateLG = (date, format) => {
    if (this.service) return this.service.formatDateLG(date, format);
  };
  formatTime = (time, format) => {
    if (this.service) return this.service.formatTime(time, format);
  };
  formatCurrency = (number, decimals) => {
    if (this.service) return this.service.formatCurrency(number, decimals);
  };

  // Events:======================================================================
  on_input_change = item => {
    item.Entry_State = 1;
  };

  // Hooks:=======================================================================
  AFTER_LOAD = entity => {};

  AFTER_CREATE = instance => {};

  AFTER_CREATE_AND_CHECKOUT = entity => {};

  BEFORE_SAVE = async entity => {};

  AFTER_SAVE(entity) {}

  AFTER_REMOVE = entity => {};

  BEFORE_CHECKIN = async () => {};

  ON_CHANGE = (data, field) => {
    this.props.onChange && this.props.onChange(data, field);
  };

  render() {
    return null;
  }
}

FormContainer.contextType = GlobalContext;

export default FormContainer;
