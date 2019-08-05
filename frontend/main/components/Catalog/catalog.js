import React from 'react';
import { withRouter } from 'next/router';
import { NoSsr, Typography, Grid } from '@material-ui/core';
import FormContainer from '../../core/FormContainer';
import { Container } from '@material-ui/core';
import { TextField } from '@material-ui/core';
import { InputBase } from '@material-ui/core';

import CatalogService from './catalog.service';
///start:slot:dependencies<<<
import { FormControlLabel, Checkbox } from '@material-ui/core';
import Select from '../../widgets/Select';
///end:slot:dependencies<<<

const service = new CatalogService();
const defaultConfig = {
  service
  ///start:slot:config<<<///end:slot:config<<<
};

class CatalogForm extends FormContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
    this.state.baseEntity.ConvertedMeta = {};
  }

  componentDidMount() {
    console.log('Form did mount');
    this.load(this.props.data);
    ///start:slot:didMount<<<
    if (this.props.parentType) {
      this.service.GetPaged(0, 1, '?CatalogType=' + this.props.parentType).then(parents => {
        this.setState({ parents: parents.Result });
      });
    }
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
  handleAutocompleteChange = (value, field) => {
    let baseEntity = this.state.baseEntity;
    baseEntity[field] = value ? value.Id : null;
    baseEntity.ParentValue = value ? value.label : null;
    this.setState({ baseEntity });
    this.ON_CHANGE(baseEntity);
  };
  ///end:slot:js<<<

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    const { additionalFields } = this.props;
    const { ConvertedMeta } = this.state.baseEntity;

    return (
      <NoSsr>
        <Container style={{ padding: 10 }}>
          <TextField
            type='text'
            label='Type'
            value={this.state.baseEntity.CatalogType || ''}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled
            readOnly
            fullWidth
          />
          <TextField
            type='text'
            label='Value'
            value={this.state.baseEntity.Value || ''}
            onChange={event => this.handleInputChange(event, 'Value')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          {this.props.parentType && (
            <Select
              flat
              label='Parent'
              options={this.state.parents}
              value={this.state.baseEntity.ParentValue}
              onChange={event => this.handleAutocompleteChange(event, 'ParentId')}
              style={{ marginTop: 20 }}
            />
          )}
          <FormControlLabel
            style={{ marginTop: 20 }}
            control={
              <Checkbox
                color='primary'
                onChange={event => this.handleCheckBoxChange(event, 'Hidden')}
                checked={this.state.baseEntity.Hidden == 1}
                value={this.state.baseEntity.Hidden}
              />
            }
            label='Hidden'
            labelPlacement='end'
          />

          {additionalFields && additionalFields.length > 0 && (
            <Typography variant='h5' style={{ marginTop: 30 }}>
              Additional Fields
            </Typography>
          )}
          {additionalFields &&
            additionalFields.map(field => (
              <TextField
                key={field.FieldName}
                type={field.FieldType}
                label={field.FieldName}
                value={ConvertedMeta[field.FieldName] || ''}
                onChange={event => {
                  let { baseEntity } = this.state;
                  baseEntity.ConvertedMeta[field.FieldName] = event.target.value;
                  this.setState({ baseEntity });
                }}
                InputLabelProps={{ shrink: true }}
                margin='normal'
                fullWidth
              />
            ))}
          <div style={{ height: 200 }} />
        </Container>
      </NoSsr>
    );
  }
}

export default withRouter(CatalogForm);
