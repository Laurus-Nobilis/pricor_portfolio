<?php
namespace App\Controller;

use App\Controller\AppController;
use Cake\ORM\TableRegistry;

/**
 * User Controller
 *
 *
 * @method \App\Model\Entity\User[]|\Cake\Datasource\ResultSetInterface paginate($object = null, array $settings = [])
 */
class UserController extends AppController
{
    public function test()
    {
        $this->autoRender	= false;

        $req_data=1;

        $userid = 2;
        $query_useritem = TableRegistry::get('useritem');
        $items = $query_useritem->find()->where(['user_id' => $userid]);
        echo "start -- \n";
        foreach ($items as $key => $value) {
            //key == indexみたいな、　value == 本体。
            //value = { "id": 1, "user_id": 2, "item_id": 1, "num": 1 }
            if($value["item_id"] == $req_data)
            {
                
            }
        }
        echo "\n -- end\n";
        return ;


        $id = 1;
        $query = TableRegistry::get('userdata');
        $user = $query->find()->where(['id' => $id])->first();
        //TODO: ユーザーの所有物(アイテム、ユニット)のデータを取り出して繋げる。
        $query_useritem = TableRegistry::get('useritem');
        $items = $query_useritem->find()->where(['user_id' => $id]);
        $query_userunit = TableRegistry::get('userunit');
        $units = $query_userunit->find()->where(['user_id' => $id]);
        
        $user = json_decode(json_encode($user), true);
        $items = json_decode(json_encode($items), true);
        $units = json_decode(json_encode($units), true);
        $user += array('items'=>$items);
        $user += array('units'=>$units);
        echo json_encode($user);


        echo json_encode($id);

        echo "--test--";
        echo "\n";

        $items = TableRegistry::get('item');

        $ret = $items->find('all');
        echo json_encode($ret);
        echo "\n";

        echo json_encode(TableRegistry::get('userdata')->find('all'));
    }

    /*
    // ユーザーチェック。//初期値=ユーザー作成は本来分けると思う。signupを作るべきだと思う。
    //　本来は検証をやるのだろう。
        user_idが来たら登録されてるやつを返す。見つからなかったらエラー
        nameが来たら新規登録として処理する。
        それ以外はエラー
    */
    public function login()
    {
        $this->autoRender = false;

        error_log("This call login");

        if (isset($this->request->data['user_id']) && $this->request->data['user_id'] > 0) {
            error_log("This call login in user id.");

            $id = $this->request->data['user_id'];
            $query = TableRegistry::get('userdata');
            $user = $query->find()->where(['id' => $id])->first();

            //ユーザー所有の (アイテム、ユニット) のデータを取り出して繋げる。
            $query_useritem = TableRegistry::get('useritem');
            $items = $query_useritem->find()->where(['user_id' => $id]);
            $query_userunit = TableRegistry::get('userunit');
            $units = $query_userunit->find()->where(['user_id' => $id]);

            $user = json_decode(json_encode($user), true);
            $items = json_decode(json_encode($items), true);
            $units = json_decode(json_encode($units), true);
            $user += array('items'=>$items);
            $user += array('units'=>$units);
            
            echo json_encode($user);
        }
        else if(isset($this->request->data['user_name'])) {
            error_log("This call login in user name.");

            $name = $this->request->data['user_name'];
            $data = array('name' => $name, 'rank'=>1, 'exp'=>0, 'lastlogin' => date('y/m/d H:i:s'), 'created'=> date('y/m/d H:i:s'));

            $query = TableRegistry::get('userdata');
            // このクラスと自動対応している DBテーブルへの参照は，このclassのコメントに @property として記述されてる。
            $userdata = $query->newEntity();
            // 追加は、Entityと、data を渡す。
            $userdata = $query->patchEntity($userdata, $data);
            try{
                //  保存処理_結果のチェック
                if ($query->saveOrFail($userdata))
                {
                    error_log($userdata);

                    echo json_encode($userdata);
                    return;
                }
            } catch (\Cake\ORM\Exception\PersistenceFailedException $e) {
                echo $e;
                //  echo $e->getEntity();
                return '500(Save Failed)';
            }
            echo "failed";
        }
        else {
            echo 'error';
        }
    }


    public function addItem()
    {
        $this->autoRender = false;
        error_log("This call addItem");

        if (isset($this->request->data['user_id']) && $this->request->data['user_id'] > 0) {

            //TODO: リクエスト内容を見て、 useritemに 追加。本来はマスターを参照したりするが、時短のため省略して来たものを入れる。
            $user_id=$this->request->data['user_id'];
            $item_id = $this->request->data['item_id'];

            $query = TableRegistry::get('useritem');
            $items = $query->find()->where(['user_id' => $user_id, 'item_id' => $item_id]);

            //存在チェック
        //  $items['id']

            //データを作る。
            //同じアイテムがあれば増減をする事になる。
            //	id, user_id, item_id, num
            $num = $this->request->data['num'];
            $data = array('user_id' => $user_id, 'item_id'=>$item_id, 'num'=>$num);

            //！！　レコード上書きの時は、これをしない。
            $userdata = $query->newEntity();

            $userdata = $query->patchEntity($userdata, $data);
            try{
                if ($query->saveOrFail($userdata))
                {
                    error_log($userdata);
                    echo json_encode($userdata);
                    return;
                }
            } catch (\Cake\ORM\Exception\PersistenceFailedException $e) {
                echo $e;
                //  echo $e->getEntity();
                return '500(Save Failed)';
            }
        }
        else {
            return 'Failed user_id error.';
        }

        return "Failed save";
    }

    /**
     * Index method
     *
     * @return \Cake\Http\Response|void
     */
    public function index()
    {
        $user = $this->paginate($this->User);

        $this->set(compact('user'));
    }

    /**
     * View method
     *
     * @param string|null $id User id.
     * @return \Cake\Http\Response|void
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function view($id = null)
    {
        $user = $this->User->get($id, [
            'contain' => []
        ]);

        $this->set('user', $user);
    }

    /**
     * Add method
     *
     * @return \Cake\Http\Response|null Redirects on successful add, renders view otherwise.
     */
    public function add()
    {
        $user = $this->User->newEntity();
        if ($this->request->is('post')) {
            $user = $this->User->patchEntity($user, $this->request->getData());
            if ($this->User->save($user)) {
                $this->Flash->success(__('The user has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The user could not be saved. Please, try again.'));
        }
        $this->set(compact('user'));
    }

    /**
     * Edit method
     *
     * @param string|null $id User id.
     * @return \Cake\Http\Response|null Redirects on successful edit, renders view otherwise.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function edit($id = null)
    {
        $user = $this->User->get($id, [
            'contain' => []
        ]);
        if ($this->request->is(['patch', 'post', 'put'])) {
            $user = $this->User->patchEntity($user, $this->request->getData());
            if ($this->User->save($user)) {
                $this->Flash->success(__('The user has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The user could not be saved. Please, try again.'));
        }
        $this->set(compact('user'));
    }

    /**
     * Delete method
     *
     * @param string|null $id User id.
     * @return \Cake\Http\Response|null Redirects to index.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function delete($id = null)
    {
        $this->request->allowMethod(['post', 'delete']);
        $user = $this->User->get($id);
        if ($this->User->delete($user)) {
            $this->Flash->success(__('The user has been deleted.'));
        } else {
            $this->Flash->error(__('The user could not be deleted. Please, try again.'));
        }

        return $this->redirect(['action' => 'index']);
    }
}
