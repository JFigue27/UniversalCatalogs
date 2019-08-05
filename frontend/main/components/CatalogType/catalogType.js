import React from 'react';
import { withRouter } from 'next/router';
import { NoSsr, Typography, Grid } from '@material-ui/core';
import FormContainer from '../../core/FormContainer';
import { Container } from '@material-ui/core';
import { TextField } from '@material-ui/core';

import CatalogTypeService from './catalogtype.service';
///start:slot:dependencies<<<
import AdditionalFields from '../AdditionalField/additionalFields';
import Select from '../../widgets/Select';
///end:slot:dependencies<<<

const service = new CatalogTypeService();
const defaultConfig = {
  service
  ///start:slot:config<<<///end:slot:config<<<
};

class CatalogTypeForm extends FormContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
  }

  componentDidMount() {
    console.log('Form did mount');
    this.load(this.props.data.Id);
    ///start:slot:didMount<<<
    this.service.LoadEntities().then(parents => {
      this.setState({ parents });
    });
    ///end:slot:didMount<<<
  }

  AFTER_LOAD = entity => {
    console.log('AFTER_LOAD', entity);
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);

    ///start:slot:afterCreate<<<///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_SAVE = entity => {
    console.log('AFTER_SAVE', entity);
    const { dialog } = this.props;
    if (dialog) dialog.close('ok');
    ///start:slot:afterSave<<<///end:slot:afterSave<<<
  };

  BEFORE_CHECKIN = () => {
    console.log('BEFORE_CHECKIN');
    ///start:slot:beforeCheckin<<<///end:slot:beforeCheckin<<<
  };

  ///start:slot:js<<<
  onFieldsChange = data => {
    let baseEntity = this.state.baseEntity;
    baseEntity.ConvertedFields = data;
    this.setState({ baseEntity });
  };
  ///end:slot:js<<<

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    return (
      <NoSsr>
        <Container style={{ padding: 20 }}>
          <TextField
            type='text'
            label='Name'
            value={this.state.baseEntity.Name || ''}
            onChange={event => this.handleInputChange(event, 'Name')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <Select
            flat
            label='Parent Type'
            value={this.state.baseEntity.ParentType}
            onChange={event => this.handleAutocompleteChange(event, 'ParentType')}
            options={this.state.parents}
          />
          <Typography variant='h5' style={{ marginTop: 30 }}>
            Additional Fields
          </Typography>
          {this.state.baseEntity.Name && <AdditionalFields parent={this.state.baseEntity} onChange={this.onFieldsChange} />}
          <div style={{ height: 200 }} />
        </Container>
      </NoSsr>
    );
  }
}

export default withRouter(CatalogTypeForm);
