<?php
namespace App\Controller;

use App\Controller\AppController;
use Cake\ORM\TableRegistry;
/**
 * Quest Controller
 *
 *
 * @method \App\Model\Entity\Quest[]|\Cake\Datasource\ResultSetInterface paginate($object = null, array $settings = [])
 */
class QuestController extends AppController
{
    public function list()
    {
        error_log()
        //カテゴリで仕分ける想定だけどしない。
        $this->autoRender = false;
        $table = TableRegistry::get('quest');
        $ret = $table->find('all');
        echo json_encode($ret);
    }

    //とりま形だけ置いておく
    public function start()
    {
        //このユーザーがクエスト開始しました。
        //ドロップ品抽選
        //ログ出しとか、
    }

    public function cancel()
    {
        //クリアしませんでした、
        // デメリットが有ったり無かったり=>無しで！
    }

    public function clear()
    {
        //クリアした。　報酬の取得、初回クリア報酬とかある。クリアランクとかある。
        //　スタミナ消費確定。
        //　報酬情報

        //　クエストに入ったユーザー情報のキャッシュかなんかする方法が分からんぬ。教えてくれん。

        if(isset($this->request->data['quest_id']))
        {
            $id = $this->request->data['quest_id'];

        }
        else
        {
            echo 'error';//これじゃダメだろうが、一旦。
        }
    }

    /**
     * Index method
     *
     * @return \Cake\Http\Response|void
     */
    public function index()
    {
        $quest = $this->paginate($this->Quest);

        $this->set(compact('quest'));
    }

    /**
     * View method
     *
     * @param string|null $id Quest id.
     * @return \Cake\Http\Response|void
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function view($id = null)
    {
        $quest = $this->Quest->get($id, [
            'contain' => []
        ]);

        $this->set('quest', $quest);
    }

    /**
     * Add method
     *
     * @return \Cake\Http\Response|null Redirects on successful add, renders view otherwise.
     */
    public function add()
    {
        $quest = $this->Quest->newEntity();
        if ($this->request->is('post')) {
            $quest = $this->Quest->patchEntity($quest, $this->request->getData());
            if ($this->Quest->save($quest)) {
                $this->Flash->success(__('The quest has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The quest could not be saved. Please, try again.'));
        }
        $this->set(compact('quest'));
    }

    /**
     * Edit method
     *
     * @param string|null $id Quest id.
     * @return \Cake\Http\Response|null Redirects on successful edit, renders view otherwise.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function edit($id = null)
    {
        $quest = $this->Quest->get($id, [
            'contain' => []
        ]);
        if ($this->request->is(['patch', 'post', 'put'])) {
            $quest = $this->Quest->patchEntity($quest, $this->request->getData());
            if ($this->Quest->save($quest)) {
                $this->Flash->success(__('The quest has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The quest could not be saved. Please, try again.'));
        }
        $this->set(compact('quest'));
    }

    /**
     * Delete method
     *
     * @param string|null $id Quest id.
     * @return \Cake\Http\Response|null Redirects to index.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function delete($id = null)
    {
        $this->request->allowMethod(['post', 'delete']);
        $quest = $this->Quest->get($id);
        if ($this->Quest->delete($quest)) {
            $this->Flash->success(__('The quest has been deleted.'));
        } else {
            $this->Flash->error(__('The quest could not be deleted. Please, try again.'));
        }

        return $this->redirect(['action' => 'index']);
    }
}
