
const updateModal = document.getElementById('modalUpdate');
const formUpdate = updateModal.querySelector('form');
const archivosUpdate = document.getElementById('archivosUpdate');
const listaArchivosUpdate = updateModal.querySelector('#listaArchivosUpdate');

const modalTitle = updateModal.querySelector('.modal-title');
const tituloUpdate = updateModal.querySelector('#tituloUpdate');
const subtituloUpdate = updateModal.querySelector('#subtituloUpdate');
const linkUpdate = updateModal.querySelector('#linkUpdate');
const idSeccionUpdate = updateModal.querySelector('#idSeccion');

let archivosList = [];
let listTipoArchivo = [];
let listTipoFile = [];

formUpdate.addEventListener('submit', (e) => {
    
    e.preventDefault();
    const formData = new FormData();
    formData.append('titulo', tituloUpdate.value);
    formData.append('subtitulo', subtituloUpdate.value);
    formData.append('link', linkUpdate.value);
    formData.append('idSeccion', idSeccionUpdate.value);
    
    listTipoFile.forEach(element => {
        formData.append('archivos', element);
    });

   
    console.log(formData);
    fetch('/Module/updateSeccion' , {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .then(data => {
         
        const {cursoId , moduloId} = data;

        if(cursoId){

            Swal.fire({
                icon: 'success',
                title: 'Exito!',
                text: 'Seccion agregada con exito!',
              })
              .then(() => {
                window.location.href = `/Module/verInfoCurso/${cursoId}?idmodulo=${moduloId}`;
              });
        }else {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Algo salio mal!',
              })
        }

    })

});

archivosUpdate.addEventListener('change', (event) => {
   
   var fileUpdate = event.target.files[0];

   if(fileUpdate == undefined){
          
          listFileSeccion();
    
    }else {

        var nameUpdate = fileUpdate.name;

        const existsInArray = archivosList.find(element => element.name === nameUpdate);

        if(existsInArray){
            
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'El archivo que intentas subir ya existe!',
              })

        }else {

            archivosList.push(fileUpdate);
            listTipoFile.push(fileUpdate);
            listFileSeccion();
        }
    }

});

updateModal.addEventListener('show.bs.modal',async event => {
              
    await logicalModalUpdate(event)

})

const logicalModalUpdate = async (event) => {
   
   
    const button = event.relatedTarget
    const idSeccion = button.getAttribute('data-bs-whatever')
    console.log(idSeccion);


    const data = await callApigetSecion(idSeccion)
    const archivoFile = await callApiGetArhivos(idSeccion)
    
    listTipoArchivo = archivoFile.map(element => {
        return element;
    })

    console.log(listTipoArchivo);
    
    const {seccion} = data;
    const {id , titulo , subtitulo , linkClase} = seccion;
    
    
    
    archivosList = archivoFile.map(element => {
        var contentType = "application/" + element.extension;
        var blob = new Blob([element.archivo], {type: contentType});
        return new File([blob], element.nombreArchivo, {type: contentType});
    });
     

     
    console.log(seccion);
    console.log(archivosList);
  

    modalTitle.textContent = "Formulario de Actualizacion de la Seccion";
    tituloUpdate.value = titulo;
    subtituloUpdate.value = subtitulo;
    linkUpdate.value = linkClase;
    idSeccionUpdate.value = id;
    listFileSeccion();

}

const listFileSeccion = () => {

    listaArchivosUpdate.innerHTML = '';

    archivosList.forEach(element => {
        const li = document.createElement('li');
        
        li.addEventListener('click', (e) => {
             
            e.preventDefault();
             
            Swal.fire({
                title: '¿Estas seguro que deseas eliminar el archivo?',
                text: "No podras revertir esta accion!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Si, Eliminar!'
              }).then((result) => {
                if (result.value) {
                  
                    archivosList = archivosList.filter(item => {
                        return item.name != element.name;
                    });

                    
                    const verdad = listTipoArchivo.find((item) => {
        
                        if(item.nombreArchivo == element.name){
                            
                            return true;
                        }
                    });

                    if(verdad){

                        const listTipo = listTipoArchivo.find((item) => {
                                        
                            if(item.nombreArchivo == element.name){
                                
                                return item;
                            }
                        });

                        callApiDeleteArchivo(listTipo.id);
                        li.remove();

                    }

                    listTipoArchivo = listTipoArchivo.filter(item => {
                        return item.nombreArchivo != element.name;
                    });


                   

                    console.log(listTipoArchivo);

                    listTipoFile = listTipoFile.filter(item => {
                        return item.name != element.name;
                    });

                    console.log(listTipoFile);

                     li.remove();

                  Swal.fire(
                    'Eliminado!',
                    'El archivo ha sido eliminado.',
                    'success'
                  )
                }
              })


        });

           li.innerHTML = element.name;
           li.classList.add('list-group-item' , 'puntero');
           listaArchivosUpdate.appendChild(li);
    });
}

const callApiDeleteArchivo = async (idArchivo) => {
     
    const url = `https://localhost:5001/Module/deleteArchivo?id=${idArchivo}`;
    const response = await fetch(url)
    const data = await response.json()
    console.log(data);

}

const callApigetSecion = async (id) => {
   
    const url = `https://localhost:5001/Module/getSeccionById?id=${id}`

    const response = await fetch(url);
    const data = await response.json();
    return data;

}

const callApiGetArhivos = async (id) => {
    const url = `https://localhost:5001/Module/getArchivosBySeccion?id=${id}`

    const response = await fetch(url);
    const data = await response.json();
    return data;

}