import { withRouter } from 'next/router';
import { NoSsr, Grid, Button, Typography, TextField } from '@material-ui/core';
import { InputBase } from '@material-ui/core';

import ItemFormContainer from './item.form.container';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const config = {
  ///start:slot:config<<<///end:slot:config<<<
};

class Item extends ItemFormContainer {
  constructor(props) {
    super(props, config);
  }

  componentDidMount() {
    console.log('Form did mount');

    ///start:slot:didMount<<<
    this.load(this.props.data.Id);
    ///end:slot:didMount<<<
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
      <NoSsr>
        <Grid className='' container direction='column' item xs={12}>
          <TextField
            type='text'
            label='Item Number'
            value={this.state.baseEntity.ItemNumber || ''}
            onChange={event => this.handleInputChange(event, 'ItemNumber')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
            fullWidth
          />
          <TextField
            type='text'
            label='Item Description'
            value={this.state.baseEntity.ItemDescription || ''}
            onChange={event => this.handleInputChange(event, 'ItemDescription')}
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

export default withRouter(Item);
