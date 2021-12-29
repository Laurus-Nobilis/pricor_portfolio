<?php
namespace App\Controller;

use App\Controller\AppController;

/**
 * Userdata Controller
 *
 * @property \App\Model\Table\UserdataTable $Userdata
 *
 * @method \App\Model\Entity\Userdata[]|\Cake\Datasource\ResultSetInterface paginate($object = null, array $settings = [])
 */
class UserdataController extends AppController
{

    /**
     * Index method
     *
     * @return \Cake\Http\Response|void
     */
    public function index()
    {
        $userdata = $this->paginate($this->Userdata);

        $this->set(compact('userdata'));
    }

    /**
     * View method
     *
     * @param string|null $id Userdata id.
     * @return \Cake\Http\Response|void
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function view($id = null)
    {
        $userdata = $this->Userdata->get($id, [
            'contain' => []
        ]);

        $this->set('userdata', $userdata);
    }

    /**
     * Add method
     *
     * @return \Cake\Http\Response|null Redirects on successful add, renders view otherwise.
     */
    public function add()
    {
        $userdata = $this->Userdata->newEntity();
        if ($this->request->is('post')) {
            $userdata = $this->Userdata->patchEntity($userdata, $this->request->getData());
            if ($this->Userdata->save($userdata)) {
                $this->Flash->success(__('The userdata has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The userdata could not be saved. Please, try again.'));
        }
        $this->set(compact('userdata'));
    }

    /**
     * Edit method
     *
     * @param string|null $id Userdata id.
     * @return \Cake\Http\Response|null Redirects on successful edit, renders view otherwise.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function edit($id = null)
    {
        $userdata = $this->Userdata->get($id, [
            'contain' => []
        ]);
        if ($this->request->is(['patch', 'post', 'put'])) {
            $userdata = $this->Userdata->patchEntity($userdata, $this->request->getData());
            if ($this->Userdata->save($userdata)) {
                $this->Flash->success(__('The userdata has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The userdata could not be saved. Please, try again.'));
        }
        $this->set(compact('userdata'));
    }

    /**
     * Delete method
     *
     * @param string|null $id Userdata id.
     * @return \Cake\Http\Response|null Redirects to index.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function delete($id = null)
    {
        $this->request->allowMethod(['post', 'delete']);
        $userdata = $this->Userdata->get($id);
        if ($this->Userdata->delete($userdata)) {
            $this->Flash->success(__('The userdata has been deleted.'));
        } else {
            $this->Flash->error(__('The userdata could not be deleted. Please, try again.'));
        }

        return $this->redirect(['action' => 'index']);
    }
}
