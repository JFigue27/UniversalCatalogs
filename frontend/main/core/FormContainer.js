import React from 'react';
import _ from 'lodash';
import AuthService from './AuthService';

class FormContainer extends React.Component {
  state = {
    config: {
      service: null
    },
    isLoading: false,
    baseEntity: {}
  };

  constructor(props, config) {
    super(props);
    if (config) Object.assign(this.state.config, config);
    this.service = this.state.config.service;

    AuthService.ON_LOGIN = this.refresh;
  }

  // Service Operations:==========================================================

  load = async entityOrId => {
    return await this.refreshForm(entityOrId);
  };

  refreshForm = async entityOrId => {
    const id = entityOrId;
    const entity = entityOrId;

    // this.setState({
    //   isLoading: true
    // });

    if (entityOrId === null) {
      this.setState({
        baseEntity: {},
        isLoading: false
      });
    }
    //Open by ID
    else if (!isNaN(id) && id > 0) {
      //TODO: Catch non-existent record
      return this.service.LoadEntity(id).then(entity => {
        this.AFTER_LOAD(entity);
        this.setState({
          isLoading: false
        });
      });
    }
    //Create
    else if ((entity instanceof Object || typeof entity == 'object') && !entity.hasOwnProperty('Id')) {
      return this.createInstance(entity);
    }
    //Open direct object
    else if (entity instanceof Object || typeof entity == 'object') {
      this.service.ADAPTER_IN(entity);
      this.AFTER_LOAD(entity);
      this.setState({
        baseEntity: entity,
        isLoading: false
      });
    }
  };

  createInstance = async (event, predefined) => {
    // let theArguments = Array.prototype.slice.call(arguments);
    return await this.service.CreateInstance(predefined).then(instance => {
      // theArguments.unshift(instance);
      // this.AFTER_CREATE.apply(this, theArguments);
      instance.editMode = true;
      instance.isDisabled = false;
      this.AFTER_CREATE(instance);
      this.setState({
        baseEntity: instance
      });
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
    return await this.service.Save(this.state.baseEntity).then(entity => {
      this.AFTER_SAVE(entity);
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
  };

  take = async (entity, toUser) => {
    return await this.service.Take(entity, toUser).then(() => {
      entity.assignedTo = toUser.Value;
      entity.AssignationMade = false;
    });
  };

  remove = async (event, entity) => {
    if (event) event.stopPropagation();
    if (confirm(`Are you sure you want to remove it?`)) {
      return await this.service.Remove(entity).then(() => {
        console.log('removed');
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
        this.load(this.baseEntity.Id);
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
      this.baseEntity.editMode = true;
      this.baseEntity.RevisionMessage = message;
      return this.BEFORE_CHECKIN().then(() => {
        return this.service.Checkin(this.state.baseEntity).then(response => {
          this.load(response).then(() => {
            this.formMode = null;
            this.baseEntity.isDisabled = true;
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
  };

  // Events:======================================================================

  on_input_change = item => {
    item.editMode = true;
  };

  // Hooks:=======================================================================
  AFTER_LOAD = entity => {};

  AFTER_CREATE = instance => {};

  AFTER_CREATE_AND_CHECKOUT = entity => {};

  AFTER_SAVE = entity => {};

  BEFORE_CHECKIN = () => {};

  render() {
    return <></>;
  }
}

export default FormContainer;
