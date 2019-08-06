import React from 'react';
import { withRouter } from 'next/router';
import { NoSsr, Typography, Grid, TextField } from '@material-ui/core';
import FormContainer from '../../core/FormContainer';
import { withSnackbar } from 'notistack';
import { InputBase } from '@material-ui/core';

import MaterialService from './material.service';

const service = new MaterialService();
const defaultConfig = {
  service
};

class MaterialForm extends FormContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
  }

  componentDidMount() {
    console.log('Form did mount');
    this.load(this.props.data.Id);
  }

  AFTER_LOAD = entity => {
    console.log('AFTER_LOAD', entity);
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
  };

  AFTER_SAVE = entity => {
    console.log('AFTER_SAVE', entity);
    const { dialog } = this.props;
    if (dialog) dialog.close('ok');
  };

  BEFORE_CHECKIN = () => {
    console.log('BEFORE_CHECKIN');
  };

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    const { isLoading, baseEntity } = this.state;

    return (
      <NoSsr>
        <Grid className='' container direction='column' item xs={12}>
          <TextField
            type='text'
            label='Material'
            value={baseEntity.Value || ''}
            onChange={event => this.handleInputChange(event, 'Value')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
        </Grid>
      </NoSsr>
    );
  }
}

export default withSnackbar(withRouter(MaterialForm));
