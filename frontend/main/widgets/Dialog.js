import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import withMobileDialog from '@material-ui/core/withMobileDialog';
import Paper from '@material-ui/core/Paper';
import Draggable from 'react-draggable';

function DraggableDialog(props) {
  return (
    <Draggable>
      <Paper {...props} />
    </Draggable>
  );
}

class DialogWidget extends React.Component {
  onOk = () => {}; //To be defined on children
  close = () => {}; //Overwritten from props.

  render() {
    const { draggable, okLabel, title, onClose, open, maxWidth, fullScreen, actionsOff, children } = this.props;
    this.close = onClose;

    return (
      <Dialog
        fullScreen={fullScreen}
        open={open || false}
        onClose={onClose}
        maxWidth={maxWidth}
        fullWidth={true}
        PaperComponent={draggable && !fullScreen ? DraggableDialog : Paper}
      >
        {title && <DialogTitle>{title}</DialogTitle>}
        <DialogContent>{children(this)}</DialogContent>
        {!actionsOff && (
          <DialogActions>
            <Button onClick={onClose} color='primary'>
              Close
            </Button>
            <Button
              onClick={() => {
                this.onOk();
              }}
              color='primary'
            >
              {okLabel || 'OK'}
            </Button>
          </DialogActions>
        )}
      </Dialog>
    );
  }
}

export default withMobileDialog()(DialogWidget);
