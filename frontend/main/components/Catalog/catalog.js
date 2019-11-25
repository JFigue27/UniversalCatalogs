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
import { withSnackbar } from 'notistack';
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
    ///start:slot:load<<<
    if (this.props.parentType) {
      this.service.GetPaged(0, 1, '?CatalogType=' + this.props.parentType).then(parents => {
        this.setState({ parents: parents.Result });
      });
    }
    ///end:slot:load<<<
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

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    const { isLoading, isDisabled, baseEntity } = this.state;

    const { additionalFields } = this.props;
    const { ConvertedMeta } = baseEntity;

    return (
      <NoSsr>
        <Container style={{ padding: 10 }}>
          <TextField
            type='text'
            label='Type'
            value={baseEntity.CatalogType || ''}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled
            readOnly
            fullWidth
          />
          <TextField
            type='text'
            label='Value'
            value={baseEntity.Value || ''}
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
              value={baseEntity.Parent}
              onChange={event => this.handleAutocompleteChange(event, 'Parent')}
              style={{ marginTop: 20 }}
            />
          )}
          <FormControlLabel
            style={{ marginTop: 20 }}
            control={
              <Checkbox
                color='primary'
                onChange={event => this.handleCheckBoxChange(event, 'Hidden')}
                checked={baseEntity.Hidden == 1}
                value={baseEntity.Hidden}
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
            additionalFields.map(field => {
              switch (field.FieldType.toLowerCase()) {
                case 'checkbox':
                  return (
                    <FormControlLabel
                      key={field.FieldName}
                      value={ConvertedMeta[field.FieldName] || ''}
                      control={<Checkbox color='primary' checked={ConvertedMeta[field.FieldName] || false} />}
                      label={field.FieldName}
                      labelPlacement='right'
                      onChange={event => {
                        ConvertedMeta[field.FieldName] = event.target.checked;
                        this.setState({ baseEntity });
                      }}
                    />
                  );
                default:
                  return (
                    <TextField
                      key={field.FieldName}
                      type={field.FieldType}
                      label={field.FieldName}
                      value={ConvertedMeta[field.FieldName] || ''}
                      onChange={event => {
                        ConvertedMeta[field.FieldName] = event.target.value;
                        this.setState({ baseEntity });
                      }}
                      InputLabelProps={{ shrink: true }}
                      margin='normal'
                      fullWidth
                    />
                  );
              }
            })}
          {/* <pre>{JSON.stringify(baseEntity, null, 3)}</pre> */}
          {/* <pre>{JSON.stringify(additionalFields, null, 3)}</pre> */}
          <div style={{ height: 200 }} />
        </Container>
      </NoSsr>
    );
  }
}

export default withSnackbar(withRouter(CatalogForm));
