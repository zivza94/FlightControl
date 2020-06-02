let {IsJson} = require('./services/utils');

module.exports =  function FilesHandler() {
  let checkedFile
  this.AddFile = (file) => {
    let reader = new FileReader()
    reader.readAsText(file)
    reader.onload = function () {
      checkedFile = reader.result
      //check if it's json file
      if (!IsJson(checkedFile)) {
        checkedFile = null
        alert(
          'This is not a valid json file!' + '\n' + 'Please choose another file'
        )
      } else {
        // this a json file, the client can submit the file.
        $('#submitBtn').attr('disabled', false)
      }
    }

    reader.onerror = function () {
      $('#submitBtn').attr('disabled', true)
    }
  }

  this.GetFile = () => {
    return checkedFile
  }
}