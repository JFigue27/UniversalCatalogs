import { withRouter } from 'next/router';
import { Grid, Button, Typography, TextField } from '@material-ui/core';

import MaterialFormContainer from './material.form.container';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const config = {
  ///start:slot:config<<<///end:slot:config<<<
};

class Material extends MaterialFormContainer {
  constructor(props) {
    super(props, config);
  }

  componentDidMount() {
    console.log('Form did mount');
    ///start:slot:didMount<<<///end:slot:didMount<<<
  }

  AFTER_SAVE = () => {
    const { dialog } = this.props;
    if (dialog) dialog.close('ok');
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    return (
      <>
        <Grid className='' container direction='column' item xs={12}>
          <TextField
            type='text'
            label='Value'
            value={this.state.baseEntity.Value || ''}
            onChange={event => this.handleInputChange(event, 'Value')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
          />
        </Grid>
      </>
    );
  }
}

export default withRouter(Material);
