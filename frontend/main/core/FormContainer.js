import React from 'react';
import AuthService from './AuthService';

class FormContainer extends React.Component {
  state = {
    config: {
      service: null
    },
    isLoading: true,
    baseEntity: {}
  };

  constructor(props, config) {
    super(props);
    if (config) Object.assign(this.state.config, config);

    this.service = this.state.config.service;

    AuthService.ON_LOGIN = this.refresh;
  }

  // Service Operations:==========================================================

  load = async criteria => {
    return await this.refreshForm(criteria);
  };

  refreshForm = async criteria => {
    //Clear form:
    if (criteria === null) {
      console.log('Form Reset.', criteria);
      this.setState({
        baseEntity: {},
        isLoading: false
      });
      this.ON_CHANGE({});
    }
    //Open by ID
    else if (!isNaN(criteria) && criteria > 0) {
      console.log('Form opened by ID.', criteria);
      //TODO: Catch non-existent record
      return this.service.LoadEntity(criteria).then(entity => {
        this.AFTER_LOAD(entity);
        this.setState({
          isLoading: false,
          baseEntity: entity
        });
        this.ON_CHANGE(entity);
      });
    }
    //Open by query parameters
    else if (typeof criteria == 'string') {
      return this.service.GetSingleWhere(null, null, criteria).then(baseEntity => {
        this.setState({ baseEntity });
        this.ON_CHANGE(baseEntity);
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
      this.service.ADAPTER_IN(criteria);
      this.AFTER_LOAD(criteria);
      this.setState({
        baseEntity: criteria,
        isLoading: false
      });
      this.ON_CHANGE(criteria);
    }
  };

  createInstance = async (event, predefined) => {
    // let theArguments = Array.prototype.slice.call(arguments);
    return await this.service.CreateInstance(predefined).then(instance => {
      // theArguments.unshift(instance);
      // this.AFTER_CREATE.apply(this, theArguments);
      instance.Entry_State = 1;
      instance.isDisabled = false;
      this.AFTER_CREATE(instance);
      this.setState({
        baseEntity: instance
      });
      this.ON_CHANGE(instance);
    });
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

  save = async () => {
    this.BEFORE_SAVE(this.state.baseEntity);
    return await this.service.Save(this.state.baseEntity).then(entity => {
      this.AFTER_SAVE(entity);
      this.ON_CHANGE(entity);
      this.setState({ baseEntity: entity });
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
    this.setState({
      baseEntity: revision
    });
    this.ON_CHANGE(revision);
  };

  take = async (entity, toUser) => {
    return await this.service.Take(entity, toUser).then(() => {
      entity.assignedTo = toUser.Value;
      entity.AssignationMade = false;
    });
  };

  remove = async (event, entity = this.state.baseEntity) => {
    if (event) event.stopPropagation();
    if (confirm(`Are you sure you want to remove it?`)) {
      return await this.service.Remove(entity).then(() => {
        this.AFTER_REMOVE(entity);
      });
    }
  };

  duplicate = async () => {
    return await this.service.Duplicate(this.state.baseEntity).then(() => {
      console.log('duplicated');
    });
  };

  checkout = async () => {
    return await this.service
      .Checkout(this.state.baseEntity)
      .then(response => {
        this.refreshForm(response);
        console.log('checked out');
      })
      .catch(() => {
        this.load(this.state.baseEntity.Id);
      });
  };

  cancelCheckout = async () => {
    return await this.service.CancelCheckout(this.state.baseEntity).then(response => {
      this.refreshForm(response);
      console.log('cancel checkout');
    });
  };

  checkin = async event => {
    if (event) event.stopPropagation();

    let message = prompt(`Optional message to reference this update.`);
    if (message !== null) {
      this.state.baseEntity.Entry_State = 1;
      this.state.baseEntity.RevisionMessage = message;
      return this.BEFORE_CHECKIN().then(() => {
        return this.service.Checkin(this.state.baseEntity).then(response => {
          this.load(response).then(() => {
            this.formMode = null;
            this.state.baseEntity.isDisabled = true;
            console.log('revision created.');
          });
        });
      });
    }
  };

  finalize = async () => {
    return await this.service
      .Finalize(this.state.baseEntity)
      .then(response => {
        this.refreshForm(response);
        console.log('finalized.');
      })
      .catch(() => {
        this.load(this.state.baseEntity.Id);
      });
  };

  unfinalize = async () => {
    return await this.service
      .Unfinalize(this.state.baseEntity)
      .then(response => {
        this.refreshForm(response);
        console.log('unfinalized.');
      })
      .catch(() => {
        this.load(this.state.baseEntity.Id);
      });
  };

  onDialogOk = async () => {
    await this.save();
  };

  // Local Operations:============================================================
  handleInputChange = (event, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = event.target.value;
    this.setState({ baseEntity });
    this.ON_CHANGE(baseEntity, field);
  };

  handleCheckBoxChange = (event, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = event.target.checked;
    this.setState({ baseEntity });
    this.ON_CHANGE(baseEntity, field);
  };

  handleDateChange = (date, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = date.toDate();
    this.setState({ baseEntity });
    this.ON_CHANGE(baseEntity, field);
  };

  handleRichText = (event, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = event.value;
    this.setState({ baseEntity });
    this.ON_CHANGE(baseEntity, field);
  };

  handleChipsChange = (value, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = value;
    this.setState({ baseEntity });
    this.ON_CHANGE(baseEntity, field);
  };

  handleAutocompleteChange = (value, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = value ? value.label : null;
    this.setState({ baseEntity });
    this.ON_CHANGE(baseEntity, field);
  };

  getCurrentUser = () => {
    return AuthService.auth.user;
  };

  getCheckoutUser = () => {
    if (this.state.baseEntity && this.state.baseEntity.CheckedoutBy) {
      return this.state.baseEntity.CheckedoutBy;
    }
  };

  _afterLoad = () => {
    let user = this.getCurrentUser();
    if (
      this.state.baseEntity &&
      this.state.baseEntity.CheckedoutBy &&
      user &&
      user.UserName &&
      this.state.baseEntity.CheckedoutBy.UserName.toLowerCase() == user.UserName.toLowerCase()
    ) {
      this.setState({
        isDisabled: false
      });
    } else {
      this.setState({
        isDisabled: true
      });
    }

    this.AFTER_LOAD(this.state.baseEntity);
  };

  clear = () => {
    this.setState({
      baseEntity: {}
    });
    this.ON_CHANGE({});
  };

  makeQueryParameters = fromObject => {
    let result = '?';
    if (fromObject instanceof Object || typeof fromObject == 'object') {
      Object.getOwnPropertyNames(fromObject).forEach(prop => {
        result += `&${prop}=${fromObject[prop]}`;
      });
    }

    return result;
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

  BEFORE_SAVE = entity => {};

  AFTER_SAVE = entity => {};

  AFTER_REMOVE = entity => {};

  BEFORE_CHECKIN = () => {};

  ON_CHANGE = (data, field) => {
    this.props.onChange && this.props.onChange(data, field);
  };

  render() {
    return <></>;
  }
}

export default FormContainer;
