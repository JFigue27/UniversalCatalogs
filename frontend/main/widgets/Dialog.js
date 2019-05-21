import React from 'react';
import PropTypes from 'prop-types';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import withMobileDialog from '@material-ui/core/withMobileDialog';
import Paper from '@material-ui/core/Paper';
import Draggable from 'react-draggable';

function PaperComponent(props) {
  return (
    <Draggable>
      <Paper {...props} />
    </Draggable>
  );
}

class ResponsiveDialog extends React.Component {
  render() {
    const { fullScreen, children, dialogOpen, fullWidth, maxWidth } = this.props;

    return (
      <div>
        <Dialog fullScreen={fullScreen} open={dialogOpen} fullWidth={fullWidth} maxWidth={maxWidth}>
          <DialogContent>{children}</DialogContent>
          <DialogActions />
        </Dialog>
      </div>
    );
  }
}

ResponsiveDialog.propTypes = {
  fullScreen: PropTypes.bool.isRequired
};

export default withMobileDialog()(ResponsiveDialog);
